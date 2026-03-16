FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["SmartSupport.Api/SmartSupport.Api.csproj", "SmartSupport.Api/"]
COPY ["SmartSupport.Application/SmartSupport.Application.csproj", "SmartSupport.Application/"]
COPY ["SmartSupport.Domain/SmartSupport.Domain.csproj", "SmartSupport.Domain/"]
COPY ["SmartSupport.Infrastructure/SmartSupport.Infrastructure.csproj", "SmartSupport.Infrastructure/"]

RUN dotnet restore "SmartSupport.Api/SmartSupport.Api.csproj"

COPY . .
WORKDIR "/src/SmartSupport.Api"
RUN dotnet publish "SmartSupport.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "SmartSupport.Api.dll"]