using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using SmartSupport.Application.Interfaces;
using SmartSupport.Infrastructure.AI.Plugins;

namespace SmartSupport.Infrastructure.AI;

public class RagService : IRagService
{
    private readonly IVectorSearchService _vectorSearchService;
    private readonly Kernel _kernel;

    public RagService(
        IVectorSearchService vectorSearchService,
        Kernel kernel, ShipmentPlugin shipmentPlugin)
    {
        _vectorSearchService = vectorSearchService;
        _kernel = kernel;

        _kernel.Plugins.AddFromObject(
            shipmentPlugin,
            "ShipmentPlugin"
            );
    }

    public async Task<string> AskAsync(string question)
    {
        if (string.IsNullOrWhiteSpace(question))
            throw new ArgumentException("Question is required.");

        // Step 1: Retrieve relevant knowledge
        var results = await _vectorSearchService.SearchAsync(question);

        // Temporary threshold for testing
        const double maxDistance = 0.45;

        var relevantResults = results
            .Where(x => x.Distance <= maxDistance)
            .ToList();

        if (relevantResults.Count == 0)
        {
            return "I don't have enough information to answer your question.";
        }

        // Step 2: Build context from retrieved answers
        var context = string.Join(
    "\n\n",
    relevantResults.Select(x =>
        $"Question: {x.Question}\nAnswer: {x.Answer}")
);

        // Step 3: Augment the prompt with context
        var prompt = """
            You are a customer support assistant.

            Answer the user's question using ONLY the
            provided knowledge base context.

            If the context does not contain the answer,
            say "I don't have enough information."

            Knowledge Base:
            {{$context}}

            User Question:
            {{$question}}
            """;

        var setting = new OpenAIPromptExecutionSettings
        {
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
        };
        // Step 4: Generate the final answer
        var arguments = new KernelArguments(setting)
        {
            ["context"] = context,
            ["question"] = question
        };

        

        var response = await _kernel.InvokePromptAsync(
            prompt,
            arguments
        );

        return response.ToString();
    }
}