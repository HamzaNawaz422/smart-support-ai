using SmartSupport.Application.DTOs;
using SmartSupport.Application.Interfaces;
using System.Text.Json;

namespace SmartSupport.Infrastructure.AI
{
    public class AiAssistantService : IAiAssistantService
    {
        private readonly ICacheService _cacheService;
        private readonly IAiKernelService _aiKernelService;

        public AiAssistantService(
            ICacheService cacheService,
            IAiKernelService aiKernelService)
        {
            _cacheService = cacheService;
            _aiKernelService = aiKernelService;
        }

        public async Task<AskResponseDto> AskAsync(AskRequestDto request)
        {
            //var cacheKey = Caching.CacheKeyHelper.GenerateQuestionCacheKey(request.Question);

            //var cachedValue = await _cacheService.GetAsync(cacheKey);

            //if (!string.IsNullOrWhiteSpace(cachedValue))
            //{
            //    var cachedResponse = JsonSerializer.Deserialize<AskResponseDto>(cachedValue);

            //    if (cachedResponse is not null)
            //    {
            //        cachedResponse.IsCached = true;
            //        return cachedResponse;
            //    }
            //}

            var answer = await _aiKernelService.GetAnswerAsync(request.Question);

            var response = new AskResponseDto
            {
                Question = request.Question,
                Answer = answer,
                IsCached = false,
                CreatedAt = DateTime.UtcNow
            };

            var serializedResponse = JsonSerializer.Serialize(response);

            //await _cacheService.SetAsync(cacheKey, serializedResponse, TimeSpan.FromHours(1));

            return response;
        }
    }
}