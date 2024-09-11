using MediatR;
using NewsAggregatorApp.Entities;
using NewsAggregatorDatabase.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorCQS.Commands.Tokens
{
	public class CreateRefreshTokenCommandHandler : IRequestHandler<CreateRefreshTokenCommand, Guid>
	{
		private readonly AggregatorContext _dbContext;

		public CreateRefreshTokenCommandHandler(AggregatorContext articleAggregatorContext)
		{
			_dbContext = articleAggregatorContext;
		}

		public async Task<Guid> Handle(CreateRefreshTokenCommand request, CancellationToken cancellationToken)
		{
			var rt = new RefreshToken()
			{
				RefreshTokenId = Guid.NewGuid(),
				DeviceInfo = "localhost",
				UserId = request.UserId,
				ExpireDateTime = DateTime.UtcNow.AddDays(1)
			};
			await _dbContext.RefreshTokens.AddAsync(rt, cancellationToken);
			await _dbContext.SaveChangesAsync(cancellationToken);

			//business logic > approach
			return rt.RefreshTokenId;
		}
	}
}
