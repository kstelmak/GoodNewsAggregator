using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregatorApp.Entities;
using NewsAggregatorDatabase.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorCQS.Queries.Tokens
{
	internal class GetRefreshTokenByIdQueryHandler : IRequestHandler<GetRefreshTokenByIdQuery, RefreshToken?>
	{
		private readonly AggregatorContext _context;

		public GetRefreshTokenByIdQueryHandler(AggregatorContext context)
		{
			_context = context;
		}

		public async Task<RefreshToken?> Handle(GetRefreshTokenByIdQuery request, CancellationToken cancellationToken)
		{
			return await _context.RefreshTokens.SingleOrDefaultAsync(refreshToken => 
			refreshToken.RefreshTokenId.Equals(request.Id), cancellationToken);
		}
	}
}
