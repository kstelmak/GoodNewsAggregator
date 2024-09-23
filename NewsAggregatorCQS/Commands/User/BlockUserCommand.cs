using MediatR;
using NewsAggregatorDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorCQS.Commands.User
{
    public class BlockUserCommand : IRequest
    {
        public string Email;
        public DateTime unblockTime;
    }
}
