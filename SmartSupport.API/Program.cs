using SmartSupport.Api.Extensions;
using SmartSupport.Application.Interfaces;
using SmartSupport.Infrastructure.DependencyInjection;
using SmartSupport.Infrastructure.Persistence;
using System.Security.Cryptography;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


//var apiKey = builder.Configuration["OpenAI:ApiKey"];

//var fingerprint = Convert.ToHexString(
//    System.Security.Cryptography.SHA256.HashData(
//        System.Text.Encoding.UTF8.GetBytes(apiKey!)
//    )
//)[..12];

//Console.WriteLine($"DOTNET KEY FINGERPRINT: {fingerprint}");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddHealthChecks();

var app = builder.Build();


//app.MapPost("/test-save-embedding", async (
//    KnowledgeDbContext db,
//    IEmbeddingService embeddingService) =>
//{
//    string question = "How can I track my shipment?";
//    string answer = "You can track your shipment using its tracking number.";

//    // 1. Generate embedding using OpenAI
//    var embedding = await embeddingService
//        .GenerateEmbeddingAsync(question);

//    // 2. Convert embedding into pgvector type
//    var entity = new SmartSupport.Infrastructure.Persistence.Entities.KnowledgeEntity
//    {
//        Question = question,
//        Answer = answer,
//        Embedding = new Pgvector.Vector(embedding.ToArray())
//    };

//    // 3. Save to PostgreSQL
//    db.KnowledgeItems.Add(entity);
//    await db.SaveChangesAsync();

//    return Results.Ok(new
//    {
//        entity.Id,
//        entity.Question,
//        entity.Answer,
//        EmbeddingDimensions = embedding.Length
//    });
//});

//app.MapGet("/test-postgres", async (KnowledgeDbContext db) =>
//{
//    bool connected = await db.Database.CanConnectAsync();

//    return new
//    {
//        Connected = connected,
//        Message = connected
//            ? "PostgreSQL connected successfully"
//            : "PostgreSQL connection failed"
//    };
//});
app.UseGlobalExceptionMiddleware();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();