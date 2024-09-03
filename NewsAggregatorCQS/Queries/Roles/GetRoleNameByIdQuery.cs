using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorCQS.Queries.Roles
{
    public class GetRoleNameByIdQuery : IRequest<string>
    {
        public Guid RoleId;
    }
}
