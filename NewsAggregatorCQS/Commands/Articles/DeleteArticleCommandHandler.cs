using MediatR;
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
    public class DeleteArticleCommandHandler : IRequestHandler<DeleteArticleCommand>
    {
        private readonly AggregatorContext _dbContext;
        private readonly ILogger<DeleteArticleCommandHandler> _logger;

        public DeleteArticleCommandHandler(AggregatorContext dbContext, ILogger<DeleteArticleCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Handle(DeleteArticleCommand command, CancellationToken cancellationToken)
        {
            var article = _dbContext.Articles.SingleOrDefault(a => a.ArticleId == command.ArticleId);
            _dbContext.Articles.Remove(article); //AddAsync существует а RemoveAsync нет?
            _logger.LogInformation($"Removed Article. ArticleId = {article.ArticleId}");
            await _dbContext.SaveChangesAsync();
        }
    }
}
