using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NewsAggregatorApp.Entities;
using NewsAggregatorCQS.Commands.Categories;
using NewsAggregatorDTOs;
using NewsAggregatorMVCModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace NewsAggregatorCQS.Commands.Articles
{
    internal class UpdateTextByWebScrappingCommandHandler : IRequestHandler<UpdateTextByWebScrappingCommand>
    {
        private readonly AggregatorContext _dbContext;
        private readonly ILogger<UpdateTextByWebScrappingCommandHandler> _logger;

        public UpdateTextByWebScrappingCommandHandler(AggregatorContext dbContext, ILogger<UpdateTextByWebScrappingCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Handle(UpdateTextByWebScrappingCommand command, CancellationToken cancellationToken)
        {
            var existingArticle = await _dbContext.Articles
                .FirstOrDefaultAsync(a => a.ArticleId == command.ArticleId);

            if (existingArticle != null)
            {
                //can be inner html
                existingArticle.Text = command.NewText;
                await _dbContext.SaveChangesAsync(cancellationToken);
                _logger.LogInformation($"UpdateText by web scrapping. ArticleId {command.ArticleId}");
            }
            else
            {
                _logger.LogWarning($"Can't UpdateText. article with ArticleId = {command.ArticleId} does not exist"); //is it trace or debug level?
            }
        }
    }
}
