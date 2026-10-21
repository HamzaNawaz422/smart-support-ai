namespace SmartSupport.Application.DTOs;

public class VectorSearchResultDto
{
    public int Id { get; set; }
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public double Distance { get; set; }
}