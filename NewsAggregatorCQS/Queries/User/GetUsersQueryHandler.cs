using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregatorApp.Entities;
using NewsAggregatorDTOs;
using NewsAggregatorMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorCQS.Queries.User
{
    internal class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, UserDto[]>
    {
        private readonly AggregatorContext _context;

        public GetUsersQueryHandler(AggregatorContext context)
        {
            _context = context;
        }

        public async Task<UserDto[]> Handle(GetUsersQuery query, CancellationToken cancellationToken)
        {
            return _context.Users
                .Include("Role")
                .Select(UserMapper.UserToUserDto).ToArray();
        }
    }
}
