using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NewsAggregatorApp.Entities;
using NewsAggregatorApp.Mappers;
using NewsAggregatorCQS.Commands.Articles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorCQS.Commands.Categories
{
    internal class InsertUniqueCategoriesFromRssDataCommandHandler : IRequestHandler<InsertUniqueCategoriesFromRssDataCommand>
    {
        private readonly AggregatorContext _dbContext;
        private readonly ILogger<InsertUniqueCategoriesFromRssDataCommandHandler> _logger;

        public InsertUniqueCategoriesFromRssDataCommandHandler(AggregatorContext dbContext, ILogger<InsertUniqueCategoriesFromRssDataCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Handle(InsertUniqueCategoriesFromRssDataCommand command, CancellationToken cancellationToken)
        {
            var existedCategoriesNames = _dbContext.Categories.Select(c => c.CategoryName).ToArray();                        
            var newUniqueCategoriesNames = command.CategoriesNames.Distinct()
                            .Where(c => !existedCategoriesNames.Contains(c))
                            .ToArray();
            if (newUniqueCategoriesNames.Length!=0)
            {
                var newUniqueCategories = newUniqueCategoriesNames.Select(c => new Category()
                {
                    CategoryId = Guid.NewGuid(),
                    CategoryName = c
                }).ToArray();

                await _dbContext.Categories.AddRangeAsync(newUniqueCategories, cancellationToken);
				await _dbContext.SaveChangesAsync(cancellationToken);
				_logger.LogInformation($"Added {newUniqueCategoriesNames.Length} categories");
            }
            else
            {
                _logger.LogInformation($"No categories added");
            }
            
        }
    }
}
