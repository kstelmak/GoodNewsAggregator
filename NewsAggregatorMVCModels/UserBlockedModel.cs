using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorMVCModels
{
    public class UserBlockedModel
    {
        public string Email { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime UnblockTime { get; set; }
    }
}
