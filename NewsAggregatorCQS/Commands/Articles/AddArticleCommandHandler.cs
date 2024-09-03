using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NewsAggregatorApp.Entities;
using NewsAggregatorApp.Mappers;
using NewsAggregatorDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorCQS.Commands.Articles
{
    public class AddArticleCommandHandler : IRequestHandler<AddArticleCommand>
    {
        private readonly AggregatorContext _dbContext;
        private readonly ILogger<AddArticleCommandHandler> _logger;

        public AddArticleCommandHandler(AggregatorContext dbContext, ILogger<AddArticleCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Handle(AddArticleCommand command, CancellationToken cancellationToken)
        {
            var article = ArticleMapper.ArticleDtoToArticle(command.ArticleDto);
            await _dbContext.Articles.AddAsync(article);
            _logger.LogInformation($"Added Article. ArticleId = {article.ArticleId}"); //is it ok to put id in logs?
            await _dbContext.SaveChangesAsync();
        }
    }
}
