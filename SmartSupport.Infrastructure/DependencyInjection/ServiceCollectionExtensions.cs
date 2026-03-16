using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartSupport.Application.Interfaces;
using SmartSupport.Infrastructure.AI;
using SmartSupport.Infrastructure.Caching;
using SmartSupport.Infrastructure.Configuration;
using StackExchange.Redis;

namespace SmartSupport.Infrastructure.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddOptions<OpenAiSettings>()
                .Bind(configuration.GetSection("OpenAI"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

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

            return services;
        }
    }
}