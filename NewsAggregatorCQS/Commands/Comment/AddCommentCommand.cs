using MediatR;
using NewsAggregatorDTOs;
using NewsAggregatorMVCModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorCQS.Commands.Comment
{
    public class AddCommentCommand : IRequest
    {
        public CommentModel Comment;
    }
}
