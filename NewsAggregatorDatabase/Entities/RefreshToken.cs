using NewsAggregatorApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorDatabase.Entities
{
	public class RefreshToken
	{
		public Guid RefreshTokenId { get; set; }
		public bool IsRevoked { get; set; }
		public string DeviceInfo { get; set; }
		public DateTime? ExpireDateTime { get; set; }


		public Guid UserId { get; set; }
		public User User { get; set; }
	}
}
