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
	internal class InsertUniqueArticlesFromRssDataCommandHandler : IRequestHandler<InsertUniqueArticlesFromRssDataCommand>
	{
		private readonly AggregatorContext _dbContext;
		private readonly ILogger<InsertUniqueArticlesFromRssDataCommandHandler> _logger;

		public InsertUniqueArticlesFromRssDataCommandHandler(AggregatorContext dbContext, ILogger<InsertUniqueArticlesFromRssDataCommandHandler> logger)
		{
			_dbContext = dbContext;
			_logger = logger;
		}

		public async Task Handle(InsertUniqueArticlesFromRssDataCommand command, CancellationToken cancellationToken)
		{
			var existedArticleUrls = await _dbContext.Articles
			.AsNoTracking()
			.Select(article => article.OriginalUrl)
			.ToArrayAsync(cancellationToken);

			var articles = command.Articles
				.Where(article => !existedArticleUrls.Contains(article.OriginalUrl))
				.Select(ArticleMapper.ArticleDtoToArticle)
				.ToArray();

			foreach(var article in articles)
			{
				article.CreationDate = DateTime.Now;
            }

            if (articles.Length != 0)
			{
				await _dbContext.Articles.AddRangeAsync(articles, cancellationToken);
				await _dbContext.SaveChangesAsync(cancellationToken);
				_logger.LogInformation($"Added {articles.Length} articles");
			}
			else
			{
				_logger.LogInformation($"No articles added");
			}
		}
	}
}
