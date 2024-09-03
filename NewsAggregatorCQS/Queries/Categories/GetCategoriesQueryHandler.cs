using MediatR;
using NewsAggregatorApp.Entities;
using NewsAggregatorCQS.Queries.Sourses;
using NewsAggregatorDTOs;
using NewsAggregatorMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorCQS.Queries.Categories
{
    public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, CategoryDto[]>
    {
        private readonly AggregatorContext _context;

        public GetCategoriesQueryHandler(AggregatorContext context)
        {
            _context = context;
        }

        public async Task<CategoryDto[]> Handle(GetCategoriesQuery query, CancellationToken cancellationToken)
        {
            return _context.Categories.Select(CategoryMapper.CategoryToCategoryDto).ToArray();
        }
    }
}
