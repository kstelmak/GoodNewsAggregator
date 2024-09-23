using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsAggregatorMVCModels;
using System.Security.Claims;
using NewsAggregatorApp.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using NewsAggregatorMapper;
using Microsoft.AspNetCore.Http.HttpResults;

namespace NewsAggregatorApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {            
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Gets user info (authorized users only) or all users (admin only)
        /// </summary>
        /// <remarks>
        /// Authorized user gets info about himself if the entered username matches the username or entered username is null
        /// Admin gets info about any user if entered username is not null. 
        /// Else admin gets all users
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">User with this name not found</response>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get(string? username, CancellationToken token = default)
        {
            if (User.IsInRole("Admin"))
            {
                if (!string.IsNullOrWhiteSpace(username))
                {
                    var model = UserMapper.UserDtoToUserModel(
                        await _userService.GetUserByNameAsync(username, new CancellationToken()));
                    if (model != null)
                    {
                        return Ok(model);
                    }
                    return NotFound("User with this name not found");
                }
                else
                {
                    var model = (await _userService.GetAllUsers(token))
                        .Select(UserMapper.UserDtoToUserModel).OrderBy(u => u.RoleName).ToArray();
                    return Ok(model);
                }
            }
            else
            {
                if (username == User.Identity.Name || string.IsNullOrWhiteSpace(username))
                {
                    var model = UserMapper.UserDtoToUserModel(
                        await _userService.GetUserByNameAsync(User.Identity.Name, new CancellationToken()));
                    return Ok(model);
                }
                return Unauthorized("You don't have permission to get info about other users");
            }
        }

        /// <summary>
        /// Registers user
        /// </summary>
        /// <returns></returns>
        /// <response code="201">User created successfully</response>
        /// <response code="400">Request is invalid</response>
        [HttpPost]
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
                //return Redirect("/login");//how to redirect?
                return Created();
            }
            return BadRequest();
        }

        /// <summary>
        /// Edits user
        /// </summary>
        /// <returns></returns>
        /// <response code="200">User edited successfully</response>
        /// <response code="400">Request is invalid</response>
        /// <response code="401">Unauthorized</response>
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Put(UserModel model, CancellationToken token = default)
        {
            if (User.Identity.Name == model.Name || User.IsInRole("Admin"))
            {
                if (await _userService.CheckIsNameRegisteredAsync(model.Name, token)
                    && !((await _userService.GetUserByEmailAsync(model.Email, token)).Name).Equals(model.Name))
                {
                    return BadRequest("name is alredy used");
                }
                await _userService.EditUserAsync(UserMapper.UserModelToUserDto(model), token);
                return Ok();
            }
            return Unauthorized("You don't have permission to edit info about other users");
        }

        /// <summary>
        /// Updates users passsword (authorized user only) or role (admin only)
        /// </summary>
        /// <remarks>
        /// Updates password for authorized user.
        /// Admin updates user role by user id.
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Passsword updated successfully</response>
        /// <response code="400">Request is invalid</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">User with this id not found</response>
        [HttpPatch]
        [Authorize]
        public async Task<IActionResult> Patch(UserChangePasswordModel? model, Guid? id, CancellationToken token = default)
        {
            if (ModelState.IsValid)
            {
                if (User.Identity.Name == (await _userService.GetUserByEmailAsync(model.Email, token)).Name)
                {
                    if (await _userService.CheckPasswordAsync(model.Email, model.OldPassword, token))
                    {
                        string passwordHash = await _userService
                            .GetPasswordHash(model.OldPassword,
                            (await _userService.GetSecurityStampAsync(model.Email, token)));
                        await _userService.ChangePasswordAsync(model.Email, passwordHash, token);
                        return Ok();
                    }
                    return BadRequest("old password is incorrect");
                }
                return Unauthorized("You don't have permission to change other users' passwords");
            }
            else if (id != null)
            {
                if (User.IsInRole("Admin"))
                {
                    if (await _userService.GetUserByIdAsync((Guid)id, token) != null)
                    {
                        await _userService.ChangeUserRoleAsync((Guid)id);
                        return Ok();
                    }
                    return NotFound("User with such this not found");
                }
                return Unauthorized();
            }
            return BadRequest();
        }
    }
}
