using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorCQS.Queries.User
{
    public class CheckPasswordQuery : IRequest<bool>
    {
        public string Email;
        public string PasswordHash;
    }
}
