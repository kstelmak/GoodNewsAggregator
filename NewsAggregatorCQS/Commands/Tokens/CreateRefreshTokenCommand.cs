using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorCQS.Commands.Tokens
{
	public class CreateRefreshTokenCommand : IRequest<Guid>
	{
		public Guid UserId { get; set; }
		public string DeviceInfo { get; set; }
	}
}
