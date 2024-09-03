//using NewsAggregatorApp.Entities;
using System.ComponentModel.DataAnnotations;

namespace NewsAggregatorMVCModels
{
    public class UserLoginModel
    {
        //[Required]
        //public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

    }
}
