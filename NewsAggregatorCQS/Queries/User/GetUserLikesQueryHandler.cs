using MediatR;
using NewsAggregatorApp.Entities;
using NewsAggregatorApp.Mappers;
using NewsAggregatorDTOs;
using NewsAggregatorMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorCQS.Queries.User
{
    internal class GetUserLikesQueryHandler : IRequestHandler<GetUserLikesQuery, LikeDto[]>
    {
        private readonly AggregatorContext _context;

        public GetUserLikesQueryHandler(AggregatorContext context)
        {
            _context = context;
        }

        public async Task<LikeDto[]> Handle(GetUserLikesQuery query, CancellationToken cancellationToken)
        {
           // _context.Likes.Where(l=>l.UserId==query.userDto.UserDtoId).Select(l=> ArticleMapper.LikeToLikeDto(l)).ToArray();
            return _context.Likes.Where(l => l.UserId == (_context.Users.SingleOrDefault(u=>u.Name==query.UserName).UserId/*query.userDto.UserDtoId*/)).Select(l => ArticleMapper.LikeToLikeDto(l)).ToArray();

            //return UserMapper.UserToUserDto(await _context.Users.SingleOrDefaultAsync(user => user.Email.Equals(query.Email)));
        }
    }
}
