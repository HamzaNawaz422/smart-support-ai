using System.Transactions;

namespace SmartSupport.Application.Services;

public static class CosineSimilarityService
{

    public static double Calculate(
        ReadOnlyMemory<float> vectorA,
        ReadOnlyMemory<float> vectorB)
    {
        if (vectorA.Length != vectorB.Length ||
            vectorA.IsEmpty)
            throw new ArgumentException("Invalid vectors");

        double dotProduct = 0;
        double magnitudeA = 0;
        double magnitudeB = 0;

        for (int i = 0; i < vectorA.Length; i++)
        {
            double a = vectorA.Span[i];
            double b = vectorB.Span[i];

            dotProduct += a * b;
            magnitudeA += a * a;
            magnitudeB += b * b;
        }

        if (magnitudeA == 0 || magnitudeB == 0)
            throw new ArgumentException("Zero vectors are invalid");

        return dotProduct /
            (Math.Sqrt(magnitudeA) * Math.Sqrt(magnitudeB));
    }
}