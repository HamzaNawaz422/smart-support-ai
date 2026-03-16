namespace SmartSupport.Application.Interfaces
{
    public interface IAiKernelService
    {
        Task<string> GetAnswerAsync(string question);
    }
}