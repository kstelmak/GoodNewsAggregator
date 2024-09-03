using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregatorApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorCQS.Queries.User
{
    public class GetUserSecurityStampQueryHandler : IRequestHandler<GetUserSecurityStampQuery, string>
    {
        private readonly AggregatorContext _context;

        public GetUserSecurityStampQueryHandler(AggregatorContext context)
        {
            _context = context;
        }

        public async Task<string> Handle(GetUserSecurityStampQuery query, CancellationToken cancellationToken)
        {
            return (await _context.Users.SingleOrDefaultAsync(u => u.Email.Equals(query.Email), cancellationToken)).SecurityStamp;
        }
    }
}
