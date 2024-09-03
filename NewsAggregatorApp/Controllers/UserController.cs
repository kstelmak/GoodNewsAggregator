using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using NewsAggregatorApp.Services.Abstractions;
using NewsAggregatorApp.Services;
using NewsAggregatorMVCModels;
using Microsoft.AspNetCore.Authorization;
using NewsAggregatorMapper;

namespace NewsAggregatorApp.Controllers
{
    public class UserController:Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginModel loginModel, CancellationToken token = default)
        {
            if (await _userService.CheckIsEmailRegisteredAsync(loginModel.Email, token) &&
                await _userService.CheckPasswordAsync(loginModel.Email, loginModel.Password))
            {
                var userRole = await _userService.GetUserRoleNameByEmailAsync(loginModel.Email, token);
                var userName = await _userService.GetUserNameByEmailAsync(loginModel.Email, token);

                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Email, loginModel.Email),
                    new Claim(ClaimTypes.Name, userName),
                    new Claim(ClaimTypes.Role, userRole),
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(principal);
                _logger.LogTrace($"User {userName} signed in");

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Incorrect Email or Password");
            //_logger.LogTrace($"User {loginModel.Email} tried signed in with");
            return View(loginModel);
        }

		[HttpGet]
		public async Task<IActionResult> Logout()
		{
            await HttpContext.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
        public IActionResult Register()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterModel registerModel, CancellationToken token = default)
        {
            if (await _userService.CheckIsEmailRegisteredAsync(registerModel.Email, token))
            {
                ModelState.AddModelError(nameof(registerModel.Email), "Email has been registered already");
                _logger.LogTrace($"Email {registerModel.Email} has been registered already");
                return View();
            }
            if (!ModelState.IsValid)
            {
                return View(registerModel);
            }

            await _userService.RegisterUserAsync(registerModel.Name, registerModel.Email, registerModel.Password, token);
            var userRole = await _userService.GetUserRoleNameByEmailAsync(registerModel.Email, token);

            var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Email, registerModel.Email),
                    new Claim(ClaimTypes.Name, registerModel.Name),
                    new Claim(ClaimTypes.Role, userRole),
                };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(principal);
            _logger.LogTrace($"User {registerModel.Name} signed in");

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<bool> CheckEmail(string Email, CancellationToken token = default)
        {
            return !(await _userService.CheckIsEmailRegisteredAsync(Email, token));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ShowUsers()
        {
            var model = (await _userService.GetAllUsers()).Select(UserMapper.UserDtoToUserModel).OrderBy(u=>u.RoleName)
                .ToArray();
            return View(model);
        }

		[HttpGet]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> ChangeRole(Guid id)
		{
            await _userService.ChangeUserRoleAsync(id);
            return RedirectToAction("ShowUsers");
		}
	}
}
