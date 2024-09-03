using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorDTOs
{
    public class UserDto
    {
        public Guid UserDtoId { get; set; }
        public Guid RoleId { get; set; }
		public string RoleName { get; set; }
		public string Name { get; set; }
        public string Email { get; set; }
    }
}
