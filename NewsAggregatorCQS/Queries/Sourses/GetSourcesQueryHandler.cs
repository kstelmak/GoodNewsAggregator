using MediatR;
using NewsAggregatorApp.Entities;
using NewsAggregatorApp.Mappers;
using NewsAggregatorCQS.Queries.Articles;
using NewsAggregatorDTOs;
using NewsAggregatorMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorCQS.Queries.Sourses
{
    public class GetSourcesQueryHandler : IRequestHandler<GetSourcesQuery, SourceDto[]>
    {
        private readonly AggregatorContext _context;

        public GetSourcesQueryHandler(AggregatorContext context)
        {
            _context = context;
        }

        public async Task<SourceDto[]> Handle(GetSourcesQuery query, CancellationToken cancellationToken)
        {
            return _context.Sources.Select(SourceMapper.SourceToSourceDto).ToArray();
        }
    }
}
