using Microsoft.EntityFrameworkCore;
using Pgvector;
using Pgvector.EntityFrameworkCore;
using SmartSupport.Application.DTOs;
using SmartSupport.Application.Interfaces;
using SmartSupport.Infrastructure.Persistence;

namespace SmartSupport.Infrastructure.AI;

public class VectorSearchService : IVectorSearchService
{
    private readonly KnowledgeDbContext _db;
    private readonly IEmbeddingService _embeddingService;

    public VectorSearchService(
        KnowledgeDbContext db,
        IEmbeddingService embeddingService)
    {
        _db = db;
        _embeddingService = embeddingService;
    }

    public async Task<List<VectorSearchResultDto>> SearchAsync(
        string question)
    {
        if (string.IsNullOrWhiteSpace(question))
            throw new ArgumentException("Question is required.");

        // Generate user question embedding
        var embedding = await _embeddingService
            .GenerateEmbeddingAsync(question);

        var queryVector = new Vector(embedding.ToArray());

        // Similarity search executed by PostgreSQL
        return await _db.KnowledgeItems
            .AsNoTracking()
            .Where(x => x.Embedding != null)
            .OrderBy(x => x.Embedding!.CosineDistance(queryVector))
            .Take(3)
            .Select(x => new VectorSearchResultDto
            {
                Id = x.Id,
                Question = x.Question,
                Answer = x.Answer,
                Distance = x.Embedding!.CosineDistance(queryVector)
            })
            .ToListAsync();
    }
}