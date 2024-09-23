using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

//using System.Web.Mvc;
//using Microsoft.AspNetCore.Mvc;


namespace NewsAggregatorMVCModels
{
    public class UserRegisterModel
    {
        [Required(ErrorMessage = "Name is required.")]
        [Remote("CheckName", "User", HttpMethod = "POST")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [Remote("CheckEmail", "User", HttpMethod = "POST")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [System.ComponentModel.DataAnnotations.Compare(nameof(Password))]
        public string PasswordConfirmation { get; set; }
    }
}
