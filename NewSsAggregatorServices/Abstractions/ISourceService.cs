using NewsAggregatorApp.Entities;
using NewsAggregatorDTOs;

namespace NewsAggregatorApp.Services.Abstractions
{
    public interface ISourceService
    {
        Task<Dictionary<Guid, string>?> GetSourcesIdsAndNamesAsync(CancellationToken token);
        Task<SourceDto?[]> GetSourcesAsync(CancellationToken token);
    }
}
