using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

//using System.Web.Mvc;
//using Microsoft.AspNetCore.Mvc;



namespace NewsAggregatorMVCModels
{
    public class UserRegisterModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [Remote("CheckEmail", "User", HttpMethod = "POST")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [System.ComponentModel.DataAnnotations.Compare(nameof(Password))]
        public string PasswordConfirmation { get; set; }
    }
}
