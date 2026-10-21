public interface IPdfTextExtractor
{
    Task<string> ExtractTextAsync(Stream pdfStream);
}