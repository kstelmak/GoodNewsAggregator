using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregatorApp.Entities;
using NewsAggregatorApp.Mappers;
using NewsAggregatorDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorCQS.Queries.Articles
{
	internal class GetArticlesByPageNumberAndPageSizeQueryHandler : IRequestHandler<GetArticlesByPageNumberAndPageSizeQuery,
	ArticleDto[]>
	{
		private readonly AggregatorContext _context;

		public GetArticlesByPageNumberAndPageSizeQueryHandler(AggregatorContext context)
		{
			_context = context;
		}

		public async Task<ArticleDto[]> Handle(GetArticlesByPageNumberAndPageSizeQuery query,
			CancellationToken token)
		{
			return await _context.Articles
				.OrderBy(article => article.Title) //order by title
				.Where(article => article.Rate >= query.MinRate)
				.Skip((query.PageNumber - 1) * query.PageSize)
				.Take(query.PageSize)// take up to pageSize 
				.Include("Source")
				.Include("Category")
				.Include("Likes")
				.AsNoTracking()
				.Select(article => ArticleMapper.ArticleToArticleDto(article))
				.ToArrayAsync(token);
		}
	}
}