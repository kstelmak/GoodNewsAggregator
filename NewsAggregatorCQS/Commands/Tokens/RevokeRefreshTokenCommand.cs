using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorCQS.Commands.Tokens
{
	public class RevokeRefreshTokenCommand : IRequest
	{
		public Guid Token { get; set; }
	}
}
