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

        public AiController(IAiAssistantService aiAssistantService)
        {
            _aiAssistantService = aiAssistantService;
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
    }
}