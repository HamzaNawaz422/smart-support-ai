namespace SmartSupport.Application.Interfaces;

public interface IRagService
{
    Task<string> AskAsync(string question);
}