using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregatorApp.Entities;
using NewsAggregatorApp.Mappers;
using NewsAggregatorCQS.Queries.Articles;
using NewsAggregatorDTOs;
using NewsAggregatorMapper;
using NewsAggregatorMVCModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorCQS.Queries.Comments
{
    internal class GetCommentsOnArticleQueryHandler: IRequestHandler<GetCommentsOnArticleQuery, CommentModel?[]>
    {
        private readonly AggregatorContext _context;

        public GetCommentsOnArticleQueryHandler(AggregatorContext context)
        {
            _context = context;
        }

        public async Task<CommentModel?[]> Handle(GetCommentsOnArticleQuery query, CancellationToken cancellationToken)
        {    
            return _context.Comments
                .Include("User")                
                .Where(c => c!.ArticleId.Equals(query.ArticleId))
                .Select(CommentMapper.CommentToCommentModel)
                .ToArray();
        }
    }
}
