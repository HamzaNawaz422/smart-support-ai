using System.Security.Cryptography;
using System.Text;

namespace SmartSupport.Infrastructure.Caching
{
    public static class CacheKeyHelper
    {
        public static string GenerateQuestionCacheKey(string question)
        {
            var normalized = question.Trim().ToLowerInvariant();

            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(normalized));
            var hash = Convert.ToHexString(bytes);

            return $"ai_response:{hash}";
        }
    }
}