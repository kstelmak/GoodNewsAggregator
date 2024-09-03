using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorCQS.Queries.User
{
    public class GetUserSecurityStampQuery:IRequest<string>
    {
        public string Email;
    }
}
