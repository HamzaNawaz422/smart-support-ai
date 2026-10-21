using Microsoft.EntityFrameworkCore;
using SmartSupport.Infrastructure.Persistence.Entities;

namespace SmartSupport.Infrastructure.Persistence;

public class KnowledgeDbContext : DbContext
{
    public KnowledgeDbContext(
        DbContextOptions<KnowledgeDbContext> options)
        : base(options)
    {
    }

    public DbSet<KnowledgeEntity> KnowledgeItems =>
        Set<KnowledgeEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("vector");

        modelBuilder.Entity<KnowledgeEntity>(entity =>
        {
            entity.ToTable("knowledge_items");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .HasColumnName("id");

            entity.Property(x => x.Question)
                .HasColumnName("question");

            entity.Property(x => x.Answer)
                .HasColumnName("answer");

            entity.Property(x => x.Embedding)
                .HasColumnName("embedding")
                .HasColumnType("vector(1536)");
        });
    }
}