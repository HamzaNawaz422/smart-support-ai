namespace SmartSupport.Application.DTOs;

public class KnowledgeItem
{
    public string Question { get; set; } = string.Empty;

    public string Answer { get; set; } = string.Empty;

    public ReadOnlyMemory<float> Embedding { get; set; }
}