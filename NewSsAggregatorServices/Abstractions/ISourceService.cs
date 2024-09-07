using NewsAggregatorApp.Entities;
using NewsAggregatorDTOs;

namespace NewsAggregatorApp.Services.Abstractions
{
    public interface ISourceService
    {
        Task<Dictionary<Guid, string>?> GetSourcesIdsAndNamesAsync();
        //Task<string?[]> GetSourcesNamesAsync();
        //Task<Guid[]?> GetSourcesIdsAsync();
        Task<SourceDto?[]> GetSourcesAsync(CancellationToken token);
    }
}
