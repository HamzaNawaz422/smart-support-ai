using SmartSupport.Application.DTOs;

namespace SmartSupport.Application.Interfaces;

public interface IVectorSearchService
{
    Task<List<VectorSearchResultDto>> SearchAsync(
        string question);
}