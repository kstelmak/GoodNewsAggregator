using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsAggregatorMVCModels;
using System.Security.Claims;
using NewsAggregatorApp.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using NewsAggregatorMapper;

namespace NewsAggregatorApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IArticleService articleService, IUserService userService, ILogger<UserController> logger)
        {
            //_articleService = articleService;
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        //gets all users or info about user
        public async Task<IActionResult> Get(string? username, CancellationToken token = default)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                var model = UserMapper.UserDtoToUserModel(
                    await _userService.GetUserByNameAsync(username, new CancellationToken()));
                return Ok(model);
            }
            else
            {
                var model = (await _userService.GetAllUsers(token))
                    .Select(UserMapper.UserDtoToUserModel).OrderBy(u => u.RoleName).ToArray();
                return Ok(model);
            }            
        }

        [HttpPost]
        //registers user
        public async Task<IActionResult> Post(UserRegisterModel registerModel, CancellationToken token = default)
        {
            if (ModelState.IsValid)
            {
                if (await _userService.CheckIsEmailRegisteredAsync(registerModel.Email, token))
                {
                    _logger.LogTrace($"Email {registerModel.Email} has been registered already");
                    return BadRequest("Email has been registered already");
                }

                await _userService.RegisterUserAsync(registerModel.Name, registerModel.Email, registerModel.Password, token);

                return Redirect("/login");//how to redirect?
            }
            return BadRequest();
        }

        [HttpPut]
        //edit user
        public async Task<IActionResult> Put(UserModel model, CancellationToken token = default)
        {
            if (await _userService.CheckIsNameRegisteredAsync(model.Name, token)
                && !(await _userService.GetUserNameByEmailAsync(model.Email, token)).Equals(model.Name)
                )
            {
                return BadRequest("name is alredy used");
            }
            else
            {
                await _userService.EditUserAsync(UserMapper.UserModelToUserDto(model), token);
            }
            return Ok();
        }

        [HttpPatch]
        //update user pass
        public async Task<IActionResult> Patch(UserChangePasswordModel model, CancellationToken token = default)
        {
            if (await _userService.CheckPasswordAsync(model.Email, model.OldPassword, token))
            {
                string passwordHash = await _userService
                    .GetPasswordHash(model.OldPassword,
                    (await _userService.GetSecurityStampAsync(model.Email, token)));
                await _userService.ChangePasswordAsync(model.Email, passwordHash, token);
                return Ok();
            }
            else
            {
                return BadRequest("old password is incorrect");
            }
        }
    }
}
