using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using SmartSupport.Application.Interfaces;
using SmartSupport.Infrastructure.Configuration;

namespace SmartSupport.Infrastructure.AI
{
    public class SemanticKernelService : IAiKernelService
    {
        private readonly Kernel _kernel;
        private readonly OpenAiSettings _settings;

        public SemanticKernelService(IOptions<OpenAiSettings> options)
        {
            _settings = options.Value;

            var builder = Kernel.CreateBuilder();

            builder.AddOpenAIChatCompletion(
                modelId: _settings.Model,
                apiKey: _settings.ApiKey);

            _kernel = builder.Build();
        }

        public async Task<string> GetAnswerAsync(string question)
        {
            var prompt = $"""
                You are a helpful AI assistant for software developers.
                Answer clearly and professionally.

                User Question:
                {question}
                """;

            var result = await _kernel.InvokePromptAsync(prompt);

            return result.ToString();
        }
    }
}