using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorMVCModels
{
    public class UserModel
    {
        public Guid UserModelId { get; set; }
        public Guid RoleId { get; set; }
        public string Name { get; set; }
		public string RoleName { get; set; }
	}
}
