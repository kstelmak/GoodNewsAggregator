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
using NewsAggregatorApp.Entities;
using System.Runtime.Intrinsics.X86;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NewsAggregatorCQS.Queries.User;
using Castle.Components.DictionaryAdapter.Xml;
using Hangfire;

namespace NewsAggregatorApp.Controllers
{
	public class UserController : Controller
	{
		//user should be able to change the password with confirmation via email

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
			HttpContext.Session.SetInt32("LoginAttempts", 0); // Инициализируем при первом заходе
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(UserLoginModel loginModel, CancellationToken token = default)
		{
			//User with this email exists
			if (await _userService.CheckIsEmailRegisteredAsync(loginModel.Email, token))
			{
				var user = await _userService.GetUserByEmailAsync(loginModel.Email, token);
				//User Is Blocked
				if (user.IsBlocked == true)
				{
					var model = new UserBlockedModel() 
					{ 
						IsBlocked = user.IsBlocked, 
						UnblockTime = (DateTime)user.UnblockTime 
					};
                    return View("Blocked", model);
				}
				//User Is not Blocked
				else
				{
					//correct Password
					if (await _userService.CheckPasswordAsync(loginModel.Email, loginModel.Password, token))
					{
						var claims = new List<Claim>()
						{
							new Claim(ClaimTypes.Email, loginModel.Email),
							new Claim(ClaimTypes.Name, user.Name),
							new Claim(ClaimTypes.Role, user.RoleName),
						};

						var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
						var principal = new ClaimsPrincipal(identity);
						await HttpContext.SignInAsync(principal);
						_logger.LogTrace($"User {user.Name} signed in");
						HttpContext.Session.SetInt32("LoginAttempts", 0);
						return RedirectToAction("Index", "Home");
					}
					//incorrect Password
					else
					{
						int loginAttempts = HttpContext.Session.GetInt32("LoginAttempts") ?? 0;
						loginAttempts++;
						//5 failed attempts
						if (loginAttempts >= 5)
						{
							var model = new UserBlockedModel()
							{
                                Email = loginModel.Email,
                                IsBlocked = true,
								UnblockTime = DateTime.Now.AddMinutes(15)
							};

							await _userService.BlockUserAsync(model, token);

							BackgroundJob.Schedule(() => _userService.UnBlockUserAsync(loginModel.Email, token),
							(model.UnblockTime.Subtract(DateTime.Now)));

							HttpContext.Session.SetInt32("LoginAttempts", 0);
							//ModelState.AddModelError("Password", "User is blocked due to multiple unsuccessful attempts.");
							return View("Blocked", model);
						}
						HttpContext.Session.SetInt32("LoginAttempts", loginAttempts);
						ModelState.AddModelError("Password", "Incorrect Password");
						return View(loginModel);
					}
				}
			}
			//User with this email not found
			else
			{
				ModelState.AddModelError("Email", "User with this email not found");
				return View(loginModel);
			}
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

			var userRole = (await _userService.GetUserByEmailAsync(registerModel.Email, token)).RoleName;

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

		//[NonAction]
		[HttpPost]
		public async Task<bool> CheckEmail(string Email, CancellationToken token = default)
		{
			return !(await _userService.CheckIsEmailRegisteredAsync(Email, token));
		}

		//[NonAction]
		[HttpPost]
		public async Task<bool> CheckName(string Name, CancellationToken token = default)
		{
			return !(await _userService.CheckIsNameRegisteredAsync(Name, token));
		}

		[HttpGet]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> ShowUsers(CancellationToken token = default)
		{
			var model = (await _userService.GetAllUsers(token)).Select(UserMapper.UserDtoToUserModel).OrderBy(u => u.RoleName)
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

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> ShowUserInfo(string username)
		{
			var model = UserMapper.UserDtoToUserModel(await _userService.GetUserByNameAsync(username, new CancellationToken()));
			return View(model);
		}

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> EditUserInfo(string username)
		{
			var model = UserMapper.UserDtoToUserModel(await _userService.GetUserByNameAsync(username, new CancellationToken()));
			return View(model);
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> EditUserInfo(UserModel model, CancellationToken token = default)
		{
			if (await _userService.CheckIsNameRegisteredAsync(model.Name, token)
				&& !((await _userService.GetUserByEmailAsync(model.Email, token)).Name).Equals(model.Name)
				)
			{
				ModelState.AddModelError("Name", "name is alredy used");
				return View(model);
			}
			await _userService.EditUserAsync(UserMapper.UserModelToUserDto(model), token);
			return View();
		}

		[Authorize]
		public async Task<IActionResult> DeleteUser(string username, CancellationToken token = default)
		{
			await _userService.DeleteUserAsync(username, token);
			return RedirectToAction("Logout");
		}

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> ChangePassword(string email)
		{
			var model = new UserChangePasswordModel() { Email = email };
			return View(model);
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> ChangePassword(UserChangePasswordModel model, CancellationToken token = default)
		{
			if (ModelState.IsValid)
			{
				if (await _userService.CheckPasswordAsync(model.Email, model.OldPassword, token))
				{
					string passwordHash = await _userService
						.GetPasswordHash(model.OldPassword,
						(await _userService.GetSecurityStampAsync(model.Email, token)));
					await _userService.ChangePasswordAsync(model.Email, passwordHash, token);
					return RedirectToAction("ShowUserInfo");
				}
				else
				{
					ModelState.AddModelError("OldPassword", "old password is incorrect");
					return View(model);
				}
			}
			return View(model);
		}
	}
}
