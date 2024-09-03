using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NewsAggregatorApp.Entities;
using NewsAggregatorApp.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorCQS.Commands.Articles
{
    public class EditArticleCommandHandler : IRequestHandler<EditArticleCommand>
    {
        private readonly AggregatorContext _dbContext;
        private readonly ILogger<EditArticleCommandHandler> _logger;

        public EditArticleCommandHandler(AggregatorContext dbContext, ILogger<EditArticleCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Handle(EditArticleCommand command, CancellationToken cancellationToken)
        {
            var article = await _dbContext.Articles
                .SingleOrDefaultAsync(article => article.ArticleId.Equals(command.ArticleDto.ArticleDtoId)); 

            article.CategoryId = command.ArticleDto.CategoryId;
            article.Title = command.ArticleDto.Title;
            article.Description = command.ArticleDto.Description;
            article.Text = command.ArticleDto.Text;
            article.Rate = command.ArticleDto.Rate;
            _logger.LogInformation($"Edited Article. ArticleId = {article.ArticleId}");

            await _dbContext.SaveChangesAsync();
        }
    }
}
