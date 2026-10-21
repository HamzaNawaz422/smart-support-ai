using SmartSupport.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartSupport.Infrastructure.AI
{
    public class TextChunkingService : ITextChunkingService
    {
        public List<string> ChunkText(
            string text,
            int chunkSize = 500,
            int overlap = 100)
        {
            if (string.IsNullOrWhiteSpace(text))
                return new List<string>();

            if (chunkSize <= 0 || overlap < 0 || overlap >= chunkSize)
                throw new ArgumentException(
                    "Chunk size must be positive and overlap must be smaller than chunk size.");

            var chunks = new List<string>();
            int step = chunkSize - overlap;

            for (int start = 0; start < text.Length; start += step)
            {
                int length = Math.Min(chunkSize, text.Length - start);

                chunks.Add(text.Substring(start, length));

                if (start + length >= text.Length)
                    break;
            }

            return chunks;
        }
    }
}
