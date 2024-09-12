using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorMVCModels
{
    public class UserChangePasswordModel
    {
        public string Email { get; set; }

        [Required]
        public string OldPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }

        [System.ComponentModel.DataAnnotations.Compare(nameof(NewPassword))]
        public string NewPasswordConfirmation { get; set; }
    }
}
