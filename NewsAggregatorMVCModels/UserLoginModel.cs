//using NewsAggregatorApp.Entities;
using System.ComponentModel.DataAnnotations;

namespace NewsAggregatorMVCModels
{
    public class UserLoginModel
    {
        //[Required]
        //public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

    }
}
