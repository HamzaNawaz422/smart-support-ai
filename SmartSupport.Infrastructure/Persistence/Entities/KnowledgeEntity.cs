using Pgvector;

namespace SmartSupport.Infrastructure.Persistence.Entities;

public class KnowledgeEntity
{
    public int Id { get; set; }
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public Vector? Embedding { get; set; }
}