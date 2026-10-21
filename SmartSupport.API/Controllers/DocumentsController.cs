using Microsoft.AspNetCore.Mvc;
using SmartSupport.Application.Interfaces;

[ApiController]
[Route("api/documents")]
public class DocumentsController : ControllerBase
{
    private readonly IPdfTextExtractor _pdfTextExtractor;
    private readonly ITextChunkingService _chunkingService;

    public DocumentsController(IPdfTextExtractor pdfTextExtractor, ITextChunkingService chunkingService)
    {
        _pdfTextExtractor = pdfTextExtractor;
        _chunkingService = chunkingService;
    }

    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Upload(
        IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Please upload a PDF file.");

        if (!string.Equals(
            Path.GetExtension(file.FileName),
            ".pdf",
            StringComparison.OrdinalIgnoreCase))
            return BadRequest("Only PDF files are allowed.");

        if (file.Length > 5 * 1024 * 1024)
            return BadRequest("Maximum file size is 5 MB.");

        await using var stream = file.OpenReadStream();

        try
        {
            var text = await _pdfTextExtractor
                .ExtractTextAsync(stream);
            var chunks = _chunkingService.ChunkText(
                        text,
                        chunkSize: 500,
                        overlap: 100);

            return Ok(new
            {
                FileName = file.FileName,
                TotalCharacters = text.Length,
                TotalChunks = chunks.Count,
                Chunks = chunks.Select((chunk, index) => new
                {
                    ChunkNumber = index + 1,
                    Text = chunk
                })
            });
        }
        catch (Exception)
        {
            return BadRequest(
                "Unable to read the PDF. Please upload a valid PDF.");
        }
    }
}