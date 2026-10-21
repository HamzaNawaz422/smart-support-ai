using System.Text;
using UglyToad.PdfPig;

public class PdfTextExtractor : IPdfTextExtractor
{
    public Task<string> ExtractTextAsync(Stream pdfStream)
    {
        var text = new StringBuilder();

        using var document = PdfDocument.Open(pdfStream);

        foreach (var page in document.GetPages())
        {
            text.AppendLine(page.Text);
        }

        return Task.FromResult(text.ToString());
    }
}