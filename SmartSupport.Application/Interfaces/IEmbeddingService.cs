using System;
using System.Collections.Generic;
using System.Text;

namespace SmartSupport.Application.Interfaces
{
    public interface IEmbeddingService
    {
        Task<ReadOnlyMemory<float>> GenerateEmbeddingAsync(string text);
    }
}
