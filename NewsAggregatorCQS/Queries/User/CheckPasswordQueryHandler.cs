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
    internal class CheckPasswordQueryHandler : IRequestHandler<CheckPasswordQuery, bool>
    {
        private readonly AggregatorContext _context;

        public CheckPasswordQueryHandler(AggregatorContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(CheckPasswordQuery query, CancellationToken cancellationToken)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email.Equals(query.Email), cancellationToken);
            if (user.PasswordHash==query.PasswordHash)
            {
                return true;
            }
            return false;
        }
    }
}
