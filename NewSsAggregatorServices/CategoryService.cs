using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NewsAggregatorApp.Entities;
using NewsAggregatorApp.Services.Abstractions;
using NewsAggregatorCQS.Queries.Categories;

namespace NewsAggregatorApp.Services
{
    public class CategoryService: ICategoryService
    {
        private readonly AggregatorContext _context;
        private readonly IMediator _mediator;
        //private readonly ILogger<CategoryService> _logger;

        public CategoryService(AggregatorContext context, IMediator mediator /*,ILogger<CategoryService> logger*/)
        {
            _context = context;
            _mediator = mediator;
            //_logger = logger;
        }

        public async Task<Dictionary<Guid, string>?> GetCategoriesIdsAndNamesAsync(CancellationToken token)
        {
            try
            {
                Dictionary<Guid, string> categoriesNames = new Dictionary<Guid, string>();
                var categories = await _mediator.Send(new GetCategoriesQuery(), token);
                foreach (var category in categories)
                {
                    categoriesNames.Add(category.CategoryDtoId, category.CategoryName);
                }
                return categoriesNames;
            }
            catch (Exception e)
            {
                throw;
            }
        }

    }
}
