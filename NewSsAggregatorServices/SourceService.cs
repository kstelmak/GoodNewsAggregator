using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NewsAggregatorApp.Entities;
using NewsAggregatorApp.Services.Abstractions;
using NewsAggregatorCQS.Queries.Sourses;
using NewsAggregatorDTOs;

namespace NewsAggregatorApp.Services
{
    public class SourceService: ISourceService
    {
        //private readonly AggregatorContext _context;
        private readonly IMediator _mediator;
        private readonly ILogger<SourceService> _logger;

        public SourceService(/*AggregatorContext context,*/ IMediator mediator, ILogger<SourceService> logger)
        {
            //_context = context;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<Dictionary<Guid, string>?> GetSourcesIdsAndNamesAsync()
        {
            try
            {
                var sources = await _mediator.Send(new GetSourcesQuery());
                Dictionary<Guid, string> sourcesIdsAndNames = new Dictionary<Guid, string>();
                foreach (var source in sources)
                {
                    sourcesIdsAndNames.Add(source.SourceDtoId, source.SourceName);
                }
                return sourcesIdsAndNames;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        //public async Task<string?[]> GetSourcesNamesAsync()
        //{
        //    try
        //    {
        //        List<string> sourcesNames = new List<string>();
        //        var sources = _context.Sources;
        //        foreach (var source in sources)
        //        {
        //            sourcesNames.Add(source.SourceName);
        //        }
        //        return sourcesNames.ToArray();
        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }
        //}

        //public async Task<Guid[]?> GetSourcesIdsAsync()//why Task<Guid?[]> doesn't work but Task<string?[]> does?
        //{
        //    try
        //    {
        //        List<Guid> sourcesIds = new List<Guid>();
        //        var sources = _context.Sources;
        //        foreach (var source in sources)
        //        {
        //            sourcesIds.Add(source.SourceId);
        //        }
        //        return sourcesIds.ToArray();
        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }
        //}

        public async Task<SourceDto?[]> GetSourcesAsync(CancellationToken token)
        {
            try
            {
                return await _mediator.Send(new GetSourcesQuery());                
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
