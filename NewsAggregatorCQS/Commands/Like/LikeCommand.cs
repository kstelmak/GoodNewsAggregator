using MediatR;
using NewsAggregatorDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorCQS.Commands.Like
{
    public class LikeCommand : IRequest
    {
        public LikeDto likeDto { get; set; }
    }
}
