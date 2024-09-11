using MediatR;
using Microsoft.Extensions.Logging;
using NewsAggregatorApp.Entities;
using NewsAggregatorCQS.Commands.Articles;
using NewsAggregatorDatabase.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorCQS.Commands.Tokens
{
	internal class RevokeRefreshTokenCommandHandler : IRequestHandler<RevokeRefreshTokenCommand>
	{
		private readonly AggregatorContext _dbContext;
		private readonly ILogger<RevokeRefreshTokenCommandHandler> _logger;

		public RevokeRefreshTokenCommandHandler(AggregatorContext articleAggregatorContext, ILogger<RevokeRefreshTokenCommandHandler> logger)
		{
			_dbContext = articleAggregatorContext;
			_logger = logger;
		}

		public async Task Handle(RevokeRefreshTokenCommand request, CancellationToken cancellationToken)
		{
			var rt = _dbContext.RefreshTokens.Where(rt=>rt.RefreshTokenId.Equals(request.Token)).First();
			rt.IsRevoked = true;
			await _dbContext.SaveChangesAsync(cancellationToken);
			_logger.LogInformation($"Token is revoked. Id {rt.RefreshTokenId}");
		}
	}
}
