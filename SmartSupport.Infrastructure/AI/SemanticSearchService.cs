using SmartSupport.Application.DTOs;
using SmartSupport.Application.Interfaces;
using SmartSupport.Application.Services;
using System.Text.Json;

namespace SmartSupport.Infrastructure.AI;

public class SemanticSearchService
{
    private readonly IEmbeddingService _embeddingService;
    private readonly ICacheService _cacheService;

    private readonly List<KnowledgeItem> _knowledgeBase = new()
    {
        new KnowledgeItem
        {
            Question = "How can I track my shipment?",
            Answer = "You can track your shipment using its tracking number."
        },
        new KnowledgeItem
        {
            Question = "How do I cancel my order?",
            Answer = "Contact customer support to cancel your order."
        },
        new KnowledgeItem
        {
            Question = "How can I change my delivery address?",
            Answer = "You can update your address before shipment dispatch."
        }
    };

    public SemanticSearchService(IEmbeddingService embeddingService, ICacheService cacheService)
    {
        _embeddingService = embeddingService;
        _cacheService = cacheService;
    }


    private async Task<ReadOnlyMemory<float>> GetOrCreateEmbeddingAsync(
    string question)
    {
        string key = $"embedding:faq:{question}";

        // Step 1: Check Redis
        string? cachedValue = await _cacheService.GetAsync(key);

        if (!string.IsNullOrEmpty(cachedValue))
        {
            float[]? cachedEmbedding =
                DeserializeEmbedding(cachedValue);

            if (cachedEmbedding is { Length: > 0 })
            {
                return cachedEmbedding; // CACHE HIT
            }
        }

        // Step 2: Generate embedding if not cached
        Console.WriteLine($"CACHE MISS: {question}");
        var embedding =
            await _embeddingService.GenerateEmbeddingAsync(question);

        // Step 3: Save to Redis for 7 days
        string json = SerializeEmbedding(embedding);

        await _cacheService.SetAsync(
            key,
            json,
            TimeSpan.FromDays(7));

        return embedding; // CACHE MISS
    }

    public async Task<(KnowledgeItem Item, double Score)> SearchAsync(
        string userQuestion)
    {
        if (string.IsNullOrWhiteSpace(userQuestion))
            throw new ArgumentException("Question is required.");

        // Convert user's question into a vector
        var userVector = await _embeddingService
            .GenerateEmbeddingAsync(userQuestion);

        KnowledgeItem? bestMatch = null;
        double highestScore = double.NegativeInfinity;

        foreach (var item in _knowledgeBase)
        {
            // Generate embedding for stored question
            item.Embedding = await GetOrCreateEmbeddingAsync(item.Question);

            // Compare both vectors
            double score = CosineSimilarityService.Calculate(
                userVector, item.Embedding);

            if (score > highestScore)
            {
                highestScore = score;
                bestMatch = item;
            }
        }

        return (bestMatch!, highestScore);
    }

    private static string SerializeEmbedding(
    ReadOnlyMemory<float> embedding)
    {
        return JsonSerializer.Serialize(embedding.ToArray());
    }

    private static float[]? DeserializeEmbedding(string json)
    {
        return JsonSerializer.Deserialize<float[]>(json);
    }
}