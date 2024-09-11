using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NewsAggregatorApp.Entities;
using NewsAggregatorApp.Services.Abstractions;
using NewsAggregatorCQS.Commands.Comment;
using NewsAggregatorCQS.Commands.User;
using NewsAggregatorCQS.Queries.Roles;
using NewsAggregatorCQS.Queries.User;
using NewsAggregatorDTOs;
using NewsAggregatorMVCModels;
using System.Security.Cryptography;
using System.Text;

namespace NewsAggregatorApp.Services
{
	//Is there any sense in testing something except ChangeUserRoleAsync in this service?
	public class UserService : IUserService
	{
		//private readonly AggregatorContext _context;
		private readonly IMediator _mediator;
		private readonly ILogger<UserService> _logger;

		public UserService(/*AggregatorContext context,*/ IMediator mediator, ILogger<UserService> logger)
		{
			//_context = context;
			_mediator = mediator;
			_logger = logger;
		}

		public async Task RegisterUserAsync(string name, string email, string password, CancellationToken token)
		{
			//var userRole = await _mediator.Send(new GetDefaultUserRoleQuery());

			//if (userRole !=null)
			//{
			var secStamp = Guid.NewGuid().ToString("N");
			var passwordHash = await GetPasswordHash(password, secStamp);
			var user = new RegisterUserDto
			{
				RegisterUserDtoId = Guid.NewGuid(),
				Name = name,
				Email = email,
				PasswordHash = passwordHash,
				SecurityStamp = secStamp,
				// RoleId = userRole
			};
			await _mediator.Send(new AddUserCommand() { registerUserDto = user }, token);


			//todo to improve: email confirm, sms, etc
			//}
			//else
			//{
			//    _logger.LogWarning("Default user role does not exist");
			//}
		}

		private async Task<string> GetPasswordHash(string password, string secStamp)
		{
			using (var md5 = MD5.Create())
			{
				var inputBytes = Encoding.UTF8.GetBytes($"{password}{secStamp}");
				var ms = new MemoryStream(inputBytes);
				var hashBytes = await md5.ComputeHashAsync(ms);
				var hashedPassword = Encoding.UTF8.GetString(hashBytes);
				return hashedPassword;
			}
		}

		public async Task<bool> CheckPasswordAsync(string email, string password, CancellationToken token)
		{
			var stamp = await _mediator.Send(new GetUserSecurityStampQuery() { Email = email }, token);

			return await _mediator.Send(new CheckPasswordQuery()
			{
				Email = email,
				PasswordHash = await GetPasswordHash(password, stamp)
			});
		}

		public async Task<bool> CheckIsEmailRegisteredAsync(string email, CancellationToken token)
		{
			//var u = (await _mediator.Send(new GetUsersQuery())).Where(u=>u.Email.Equals(email)).FirstOrDefault();
			if ((await _mediator.Send(new GetUsersQuery(), token)).Where(u => u.Email.Equals(email)).FirstOrDefault() != null)
			{
				return true;
			}
			return false;
		}

		//public async Task<string> GetUserRoleNameByIdAsync(Guid id, CancellationToken token)
		//{
		//    var userRole = (await _context.Users
		//        .Include(user => user.Role)
		//        .SingleOrDefaultAsync(user => user.UserId == id))?.Role.RoleName;

		//    return userRole;
		//}

		public async Task<string> GetUserRoleNameByEmailAsync(string email, CancellationToken token)
		{
			return await _mediator.Send(new GetRoleNameByIdQuery()
			{
				RoleId = (await _mediator.Send(new GetUsersQuery(), token))
				.Where(u => u.Email.Equals(email)).FirstOrDefault().RoleId
			});
		}

		public async Task<string> GetUserNameByEmailAsync(string email, CancellationToken token)
		{
			return (await _mediator.Send(new GetUsersQuery(), token))
				.Where(u => u.Email.Equals(email)).FirstOrDefault().Name;
		}

		public async Task<Guid> GetUserIdByEmailAsync(string email, CancellationToken token = default)
		{
			return (await _mediator.Send(new GetUsersQuery(), token))
				.Where(u => u.Email.Equals(email)).FirstOrDefault().UserDtoId;
		}

		public async Task<LikeDto[]> GetUserLikesAsync(string userName, CancellationToken token)
		{
			return await _mediator.Send(new GetUserLikesQuery() { UserName = userName }, token);
		}

		public async Task AddCommentAsync(CommentModel comment, string name, CancellationToken token = default)
		{
			comment.CommentModelId = Guid.NewGuid();
			comment.UserId = (await _mediator.Send(new GetUsersQuery(), token))
					.Where(u => u.Name.Equals(name)).FirstOrDefault().UserDtoId;
			await _mediator.Send(new AddCommentCommand() { Comment = comment }, token);
		}


		//public async Task<string> GetUserRoleByNameAsync(string name, CancellationToken token)
		//{
		//    var userRole = (await _context.Users
		//        .Include(user => user.Name)
		//        .SingleOrDefaultAsync(user => user.EMail.Equals(name),
		//            cancellationToken: token))?.Role.RoleName;

		//    return userRole;
		//}

		//public async Task<Guid?> GetUserIdByEmailAsync(string modelEmail, CancellationToken cancellationToken)
		//{
		//    return await _mediator.Send(new GetUserIdByEmailQuery() { Email = modelEmail }, cancellationToken);
		//}

		//public async Task<UserTokenDto> GetUserDataByRefreshToken(Guid id, CancellationToken cancellationToken)
		//{
		//    return await _mediator.Send(new GetUserDataByRefreshTokenQuery() { ToklenId = id }, cancellationToken);
		//}

		public async Task<UserDto[]> GetAllUsers()
		{
			return await _mediator.Send(new GetUsersQuery());
		}

		public async Task ChangeUserRoleAsync(Guid id, CancellationToken token)
		{
			var users = await _mediator.Send(new GetUsersQuery(), token);
			var user = users.Where(u => u.UserDtoId.Equals(id)).FirstOrDefault();
			if (user != null)
			{
				string newRole = "User";
				if (user.RoleName == "User")
				{
					newRole = "Admin";
				}
				await _mediator.Send(new SetRoleCommand() { UserId = id, RoleName = newRole }, token);
			}
		}

		public async Task<UserTokenDto> GetUserDataByRefreshToken(Guid id, CancellationToken cancellationToken)
		{
			return await _mediator.Send(new GetUserDataByRefreshTokenQuery() { ToklenId = id }, cancellationToken);
		}
	}
}
