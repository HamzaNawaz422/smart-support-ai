# SmartSupport.AI

AI-powered ASP.NET Core Web API using Redis caching and Semantic Kernel.

## Features
- ASP.NET Core Web API (.NET 8)
- Clean Architecture
- Redis caching
- Semantic Kernel integration
- OpenAI API
- Global exception handling
- Serilog logging
- Docker Compose support
- Health checks

## Architecture
Api -> Application -> Infrastructure -> Redis/OpenAI

## Run locally
1. Start Redis with Docker
2. Set OpenAI API key with user secrets
3. Run API
4. Open Swagger

## Example Request
POST /api/ai/ask

{
  "question": "Explain Redis caching in ASP.NET Core"
}

## Example Response
{
  "question": "Explain Redis caching in ASP.NET Core",
  "answer": "...",
  "isCached": false,
  "createdAt": "..."
}