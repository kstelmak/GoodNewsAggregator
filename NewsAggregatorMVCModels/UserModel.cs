using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public string Email { get; set; }
        public int MinRate { get; set; }

		public bool IsBlocked { get; set; }
        public DateTime? UnblockTime { get; set; }
    }
}
