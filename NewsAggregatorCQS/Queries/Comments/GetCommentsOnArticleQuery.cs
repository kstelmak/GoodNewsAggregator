using MediatR;
using NewsAggregatorDTOs;
using NewsAggregatorMVCModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorCQS.Queries.Comments
{
    public class GetCommentsOnArticleQuery : IRequest<CommentModel?[]>
    {
        public Guid ArticleId { get; set; }
    }
}
