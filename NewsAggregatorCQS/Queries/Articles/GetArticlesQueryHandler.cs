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
    public class GetArticlesQueryHandler : IRequestHandler<GetArticlesQuery, ArticleDto[]>
    {
        private readonly AggregatorContext _context;

        public GetArticlesQueryHandler(AggregatorContext context)
        {
            _context = context;
        }

        public async Task<ArticleDto[]> Handle(GetArticlesQuery query, CancellationToken cancellationToken)
        {
            var a = _context.Articles
                .Include("Source")
                .Select(ArticleMapper.ArticleToArticleDto).ToArray();

            //Include("_context.Sources").Select(ArticleMapper.ArticleToArticleDto).ToArray();

            //Select(ArticleMapper.ArticleToArticleDto).ToArray();
            return _context.Articles.Select(ArticleMapper.ArticleToArticleDto).ToArray();
        }
    }
}
