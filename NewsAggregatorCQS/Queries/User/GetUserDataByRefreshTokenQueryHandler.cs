using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregatorApp.Entities;
using NewsAggregatorDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorCQS.Queries.User
{
	internal class GetUserDataByRefreshTokenQueryHandler : IRequestHandler<GetUserDataByRefreshTokenQuery, UserTokenDto?>
	{
		private readonly AggregatorContext _context;

		public GetUserDataByRefreshTokenQueryHandler(AggregatorContext context)
		{
			_context = context;
		}

		public async Task<UserTokenDto?> Handle(GetUserDataByRefreshTokenQuery request, CancellationToken cancellationToken)
		{
			var userId = (await _context.RefreshTokens.AsNoTracking().SingleOrDefaultAsync(
				refreshToken => refreshToken.RefreshTokenId.Equals(request.ToklenId), cancellationToken))?.UserId;
			var user = await _context.Users.Include(u => u.Role)
				.AsNoTracking()
				.SingleOrDefaultAsync(u => u.UserId.Equals(userId), cancellationToken);
			if (user != null)
			{
				return new UserTokenDto()
				{
					Id = userId.Value,
					Email = user.Email,
					RoleName = user.Role.RoleName,
					RefreshToken = request.ToklenId
				};
			}
			return null;
		}
	}
}
