using MediatR;
using NewsAggregatorDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorCQS.Queries.User
{
    public class GetUserLikesQuery : IRequest<LikeDto[]>
    {
        public string UserName { get; set; }
    }
}
