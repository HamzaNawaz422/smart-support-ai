using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pgvector.EntityFrameworkCore;
using SmartSupport.Application.DTOs;
using SmartSupport.Application.Interfaces;
using SmartSupport.Application.Services;
using SmartSupport.Infrastructure.AI;
using SmartSupport.Infrastructure.Persistence;

namespace SmartSupport.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AiController : ControllerBase
    {
        private readonly IAiAssistantService _aiAssistantService;
        private readonly IEmbeddingService _embeddingService;
        private readonly SemanticSearchService _semanticSearchService;
        private readonly IVectorSearchService _vectorSearchService;
        private readonly IRagService _ragService;

        public AiController(IAiAssistantService aiAssistantService,
            IEmbeddingService embeddingService, 
            SemanticSearchService semanticSearchService, 
            IVectorSearchService vectorSearchService, IRagService ragService)
        {
            _aiAssistantService = aiAssistantService;
            _embeddingService = embeddingService;
            _semanticSearchService = semanticSearchService;
            _vectorSearchService= vectorSearchService;
            _ragService=ragService;
        }

        [HttpPost("ask")]
        public async Task<IActionResult> Ask([FromBody] AskRequestDto request)
        {
            if (request is null || string.IsNullOrWhiteSpace(request.Question))
            {
                return BadRequest("Question is required.");
            }

            var response = await _aiAssistantService.AskAsync(request);

            return Ok(response);
        }

        [HttpPost("embedding")]
        public async Task<IActionResult> GenerateEmbedding([FromBody] string text)
        {
            var embedding =
                await _embeddingService.GenerateEmbeddingAsync(text);

            return Ok(new
            {
                Text = text,
                Dimensions = embedding.Length,
                Vector = embedding.ToArray()
            });
        }

        [HttpPost("similarity")]
        public async Task<IActionResult> CompareSimilarity(
        [FromBody] SimilarityRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Text1) ||
                string.IsNullOrWhiteSpace(request.Text2))
            {
                return BadRequest("Both sentences are required.");
            }

            var vector1 = await _embeddingService
                .GenerateEmbeddingAsync(request.Text1);

            var vector2 = await _embeddingService
                .GenerateEmbeddingAsync(request.Text2);

            var score = CosineSimilarityService.Calculate(
                vector1, vector2);

            return Ok(new
            {
                request.Text1,
                request.Text2,
                SimilarityScore = Math.Round(score, 4)
            });
        }


        [HttpPost("semantic-search")]
        public async Task<IActionResult> SemanticSearch(
        [FromBody] string question)
        {
            if (string.IsNullOrWhiteSpace(question))
                return BadRequest("Question is required.");

            var (item, score) =
                await _semanticSearchService.SearchAsync(question);

            return Ok(new
            {
                UserQuestion = question,
                MatchedQuestion = item.Question,
                Answer = item.Answer,
                SimilarityScore = Math.Round(score, 4)
            });
        }

        [HttpPost("vector-search")]
        public async Task<IActionResult> VectorSearch(
        [FromBody] string question)
        {
            var response=await _vectorSearchService.SearchAsync(question);
            // PostgreSQL performs similarity search
            
            return Ok(response);
        }

        [HttpPost("ask-rag")]
        public async Task<IActionResult> AskRag(
        [FromBody] string question)
        {
            if (string.IsNullOrEmpty(question))
                return BadRequest("Question is required");
            var response = await _ragService.AskAsync(question);
            // PostgreSQL performs similarity search

            return Ok(response);
        }
    }
}