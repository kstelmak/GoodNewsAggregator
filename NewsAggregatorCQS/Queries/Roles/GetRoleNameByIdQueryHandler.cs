using MediatR;
using NewsAggregatorApp.Entities;
using NewsAggregatorCQS.Queries.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorCQS.Queries.Roles
{
    internal class GetRoleNameByIdQueryHandler : IRequestHandler<GetRoleNameByIdQuery, string>
    {
        private readonly AggregatorContext _context;

        public GetRoleNameByIdQueryHandler(AggregatorContext context)
        {
            _context = context;
        }

        public async Task<string> Handle(GetRoleNameByIdQuery query, CancellationToken cancellationToken)
        {
            return (_context.Roles.SingleOrDefault(r => r.RoleId.Equals(query.RoleId))).RoleName;
        }
    }
}
