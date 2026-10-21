using System;
using System.Collections.Generic;
using System.Text;

namespace SmartSupport.Application.Interfaces
{
    public interface ITextChunkingService
    {
        List<string> ChunkText(
            string text,
            int chunkSize = 500,
            int overlap = 100);
    }
}
