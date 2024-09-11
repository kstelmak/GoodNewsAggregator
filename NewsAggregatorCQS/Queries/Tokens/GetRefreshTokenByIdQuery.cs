using MediatR;
using NewsAggregatorDatabase.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorCQS.Queries.Tokens
{
	public class GetRefreshTokenByIdQuery : IRequest<RefreshToken?>
	{
		public Guid Id { get; set; }
	}
}
