using MediatR;
using Microsoft.Extensions.Logging;
using NewsAggregatorApp.Services.Abstractions;
using NewsAggregatorCQS.Commands.Comment;
using NewsAggregatorCQS.Commands.Like;
using NewsAggregatorCQS.Commands.User;
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
        private readonly IMediator _mediator;
        private readonly ILogger<UserService> _logger;

        public UserService(IMediator mediator, ILogger<UserService> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task RegisterUserAsync(string name, string email, string password, CancellationToken token)
        {
            var secStamp = Guid.NewGuid().ToString("N");
            var passwordHash = await GetPasswordHash(password, secStamp);
            var user = new RegisterUserDto
            {
                RegisterUserDtoId = Guid.NewGuid(),
                Name = name,
                Email = email,
                PasswordHash = passwordHash,
                SecurityStamp = secStamp,
            };
            await _mediator.Send(new AddUserCommand() { registerUserDto = user }, token);

            //todo to improve: email confirm, sms, etc
        }

        public async Task<string> GetPasswordHash(string password, string secStamp)
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

        public async Task<string> GetSecurityStampAsync(string email, CancellationToken token)
        {
            return await _mediator.Send(new GetUserSecurityStampQuery() { Email = email }, token);
        }

        public async Task<bool> CheckPasswordAsync(string email, string password, CancellationToken token)
        {
            var stamp = await GetSecurityStampAsync(email, token);

            return await _mediator.Send(new CheckPasswordQuery()
            {
                Email = email,
                PasswordHash = await GetPasswordHash(password, stamp)
            }, token);
        }

        public async Task<bool> CheckIsEmailRegisteredAsync(string email, CancellationToken token)
        {
            if (await GetUserByEmailAsync(email, token) != null)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> CheckIsNameRegisteredAsync(string name, CancellationToken token)
        {

            if (await GetUserByNameAsync(name, token) != null)
            {
                return true;
            }
            return false;
        }

        public async Task<LikeDto[]> GetUserLikesAsync(string userName, CancellationToken token)
        {
            return await _mediator.Send(new GetUserLikesQuery() { UserName = userName }, token);
        }

        public async Task AddCommentAsync(CommentModel comment, CancellationToken token)
        {
            comment.CommentModelId = Guid.NewGuid();
            comment.UserId = (await GetUserByNameAsync(comment.UserName, token)).UserDtoId;
            await _mediator.Send(new AddCommentCommand() { Comment = comment }, token);
        }

        public async Task<UserDto[]> GetAllUsers(CancellationToken token)
        {
            return await _mediator.Send(new GetUsersQuery(), token);
        }

        public async Task ChangeUserRoleAsync(Guid id, CancellationToken token)
        {
            var user = await GetUserByIdAsync(id, token);
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

        public async Task<UserTokenDto> GetUserDataByRefreshTokenAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _mediator.Send(
                new GetUserDataByRefreshTokenQuery() { ToklenId = id }, cancellationToken);
        }

        public async Task<UserDto> GetUserByNameAsync(string username, CancellationToken cancellationToken)
        {
            return (await _mediator.Send(
                new GetUsersQuery() { }, cancellationToken))
                .Where(u => u.Name.Equals(username))
                .First();
        }

        public async Task<UserDto> GetUserByEmailAsync(string Email, CancellationToken cancellationToken)
        {
            var users = (await _mediator.Send(
                new GetUsersQuery() { }, cancellationToken))
                .Where(u => u.Email.Equals(Email))
                .ToArray();

            if (users.Length!=0)
            {
                return users.First();
            }
            else
            {
                return null;
            }
        }

        public async Task<UserDto> GetUserByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return (await _mediator.Send(
                new GetUsersQuery() { }, cancellationToken))
                .Where(u => u.UserDtoId.Equals(id))
                .First();
        }

        public async Task EditUserAsync(UserDto user, CancellationToken cancellationToken)
        {
            await _mediator.Send(
                new EditUserCommand() { UserDto = user }, cancellationToken);
        }

        public async Task BlockUserAsync(UserBlockedModel model, CancellationToken cancellationToken)
        {
            await _mediator.Send(
                new BlockUserCommand() { Email = model.Email, unblockTime = (DateTime)model.UnblockTime}, 
                cancellationToken);
        }

		public async Task UnBlockUserAsync(string email, CancellationToken cancellationToken)
		{
			await _mediator.Send(
				new UnBlockUserCommand() { Email = email },
				cancellationToken);
		}

		public async Task DeleteUserAsync(string username, CancellationToken cancellationToken)
        {
            await _mediator.Send(
                new DeleteUserCommand() { Username = username }, cancellationToken);
        }

        public async Task ChangePasswordAsync(string email, string passwordHash, CancellationToken cancellationToken)
        {
            await _mediator.Send(
                new ChangePasswordCommand() { Email = email, PasswordHash = passwordHash }, cancellationToken);
        }

        public async Task LikeAsync(Guid id, string name, CancellationToken token)
        {
            var like = new LikeDto()
            {
                LikeDtoId = Guid.NewGuid(),
                UserId = (await _mediator.Send(new GetUsersQuery(), token))
                    .Where(u => u.Name.Equals(name)).FirstOrDefault().UserDtoId,
                ArticleId = id,
            };
            await _mediator.Send(new LikeCommand() { likeDto = like }, token);
        }
    }
}
