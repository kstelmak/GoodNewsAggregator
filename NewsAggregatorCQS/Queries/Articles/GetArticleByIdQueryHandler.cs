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
    public class GetArticleByIdQueryHandler : IRequestHandler<GetArticleByIdQuery, ArticleDto>
    {
        private readonly AggregatorContext _context;

        public GetArticleByIdQueryHandler(AggregatorContext context)
        {
            _context = context;
        }

        public async Task<ArticleDto> Handle(GetArticleByIdQuery query, CancellationToken cancellationToken)
        {
            var article = await _context.Articles
                .Include("Likes")
                .Include("Source")
                //.Include("Comments")
                .FirstOrDefaultAsync(a => a.ArticleId == query.ArticleId);            

            var artDto = ArticleMapper.ArticleToArticleDto(article);
            artDto.LikesCount = article.Likes.Count();
            return artDto;
        }
    }
}
