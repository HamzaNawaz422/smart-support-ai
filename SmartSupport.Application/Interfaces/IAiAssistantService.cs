using SmartSupport.Application.DTOs;

namespace SmartSupport.Application.Interfaces
{
    public interface IAiAssistantService
    {
        Task<AskResponseDto> AskAsync(AskRequestDto request);
    }
}