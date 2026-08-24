using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using SmartSupport.Application.Interfaces;
using SmartSupport.Infrastructure.AI.Plugins;
using SmartSupport.Infrastructure.Configuration;

namespace SmartSupport.Infrastructure.AI
{
    public class SemanticKernelService : IAiKernelService
    {
        private readonly Kernel _kernel;
        private readonly OpenAiSettings _settings;
        private readonly IShipmentService _shipmentService;

        public SemanticKernelService(IOptions<OpenAiSettings> options, ShipmentPlugin shipmentPlugin,
            IShipmentService shipmentService)
        {
            _settings = options.Value;

            var builder = Kernel.CreateBuilder();

            builder.AddOpenAIChatCompletion(
                modelId: _settings.Model,
                apiKey: _settings.ApiKey);

            _kernel = builder.Build();
            _kernel.Plugins.AddFromObject(
            shipmentPlugin,
        "ShipmentPlugin");
            //_kernel.Plugins.AddFromType<ShipmentPlugin>();
            _shipmentService = shipmentService;

        }

        public async Task<string> GetAnswerAsync(string question)
        {
            var prompt = $"""
                You are a helpful AI assistant for software developers.
                Answer clearly and professionally.

                User Question:
                {question}
                """;

            var executionSettings = new OpenAIPromptExecutionSettings
            {
                FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
            };

            var result = await _kernel.InvokePromptAsync
                (prompt, new KernelArguments(executionSettings));

            return result.ToString();
        }
    }
}