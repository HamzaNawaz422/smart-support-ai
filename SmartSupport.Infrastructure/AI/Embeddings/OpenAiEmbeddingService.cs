using Microsoft.Extensions.Options;
using OpenAI.Embeddings;
using SmartSupport.Application.Interfaces;
using SmartSupport.Infrastructure.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartSupport.Infrastructure.AI.Embeddings
{
    public class OpenAiEmbeddingService : IEmbeddingService
    {
        private readonly EmbeddingClient _embeddingClient;
        public OpenAiEmbeddingService(IOptions<OpenAiSettings> options)
        {
            var settings = options.Value;
            _embeddingClient = new EmbeddingClient(
                model: settings.EmbeddingModel,
                apiKey: settings.ApiKey);
        }

        public async Task<ReadOnlyMemory<float>> GenerateEmbeddingAsync(
            string text)
        {
            try
            {
                var result = await _embeddingClient.GenerateEmbeddingAsync(text);
                return result.Value.ToFloats();
            }
            catch (Exception ex)
            {

                return null;
            }
            
        }
    }
}
