using Microsoft.AspNetCore.Mvc;
using SmartSupport.Application.DTOs;
using SmartSupport.Application.Interfaces;

namespace SmartSupport.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AiController : ControllerBase
    {
        private readonly IAiAssistantService _aiAssistantService;
        private readonly IEmbeddingService _embeddingService;

        public AiController(IAiAssistantService aiAssistantService, IEmbeddingService embeddingService)
        {
            _aiAssistantService = aiAssistantService;
            _embeddingService = embeddingService;
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
    }
}