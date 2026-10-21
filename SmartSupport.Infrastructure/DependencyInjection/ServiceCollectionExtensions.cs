using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using SmartSupport.Application.Interfaces;
using SmartSupport.Infrastructure.AI;
using SmartSupport.Infrastructure.AI.Embeddings;
using SmartSupport.Infrastructure.AI.Plugins;
using SmartSupport.Infrastructure.Caching;
using SmartSupport.Infrastructure.Configuration;
using SmartSupport.Infrastructure.Persistence;
using StackExchange.Redis;

namespace SmartSupport.Infrastructure.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {

            services.AddDbContext<KnowledgeDbContext>(options =>
            options.UseNpgsql(
            configuration.GetConnectionString("PostgreSQL"),
            npgsql => npgsql.UseVector()));

            services
                .AddOptions<OpenAiSettings>()
                .Bind(configuration.GetSection("OpenAI"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddKernel()
                    .AddOpenAIChatCompletion(
                    modelId: configuration["OpenAI:Model"]!,
                    apiKey: configuration["OpenAI:ApiKey"]!
    );

            var redisConnection = configuration.GetConnectionString("Redis");

            services.AddSingleton<IConnectionMultiplexer>(_ =>
            {
                var options = ConfigurationOptions.Parse(redisConnection!, true);
                options.AbortOnConnectFail = false;
                return ConnectionMultiplexer.Connect(options);
            });

            services.AddScoped<ICacheService, RedisCacheService>();
            services.AddScoped<IAiKernelService, SemanticKernelService>();
            services.AddScoped<IAiAssistantService, AiAssistantService>();
            services.AddScoped<IShipmentService, ShipmentService>();
            services.AddScoped<ShipmentPlugin>();
            services.AddScoped<IEmbeddingService, OpenAiEmbeddingService>();
            services.AddScoped<AI.SemanticSearchService>();
            services.AddScoped<IVectorSearchService, VectorSearchService>();
            services.AddScoped<IRagService, RagService>();
            services.AddScoped<IPdfTextExtractor, PdfTextExtractor>();
            services.AddScoped<ITextChunkingService, TextChunkingService>();

            return services;
        }
    }
}