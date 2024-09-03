using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NewsAggregatorApp.Entities;
using NewsAggregatorApp.Mappers;
using NewsAggregatorCQS.Queries.Articles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorCQS.Commands.Articles
{
    internal class SetRateCommandHandler : IRequestHandler<SetRateCommand>
    {
        private readonly AggregatorContext _dbContext;
        private readonly ILogger<SetRateCommandHandler> _logger;

        public SetRateCommandHandler(AggregatorContext dbContext, ILogger<SetRateCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Handle(SetRateCommand command, CancellationToken cancellationToken)
        {
            var article = await _dbContext.Articles
                .FirstOrDefaultAsync(a => a.ArticleId == command.ArticleId);

            if (article != null)
            {
                article.Rate = command.newRate;

                _logger.LogInformation($"SetRate. ArticleId = {article.ArticleId}, rate = {command.newRate}");
            }
            _logger.LogWarning($"Can not SetRate. Article with ArticleId = {article.ArticleId} des not exist");
        }
    }
}
