using NewsAggregatorDTOs;
using NewsAggregatorMVCModels;

namespace NewsAggregatorApp.Services.Abstractions
{
    public interface IUserService
    {
        Task RegisterUserAsync(string name, string email, string password, CancellationToken token);
        Task AddCommentAsync(CommentModel model, CancellationToken token);
        Task LikeAsync(Guid id, string name, CancellationToken token);
        Task ChangeUserRoleAsync(Guid id, CancellationToken token = default);
        Task EditUserAsync(UserDto user, CancellationToken cancellationToken);
        Task DeleteUserAsync(string username, CancellationToken cancellationToken);
        Task BlockUserAsync(UserBlockedModel model, CancellationToken cancellationToken);
        Task UnBlockUserAsync(string email, CancellationToken cancellationToken);

		Task ChangePasswordAsync(string email, string passwordHash, CancellationToken cancellationToken);

        Task<bool> CheckPasswordAsync(string email, string password, CancellationToken token);
        Task<bool> CheckIsEmailRegisteredAsync(string email, CancellationToken token);
        Task<bool> CheckIsNameRegisteredAsync(string name, CancellationToken token);

        Task<string> GetPasswordHash(string password, string secStamp);
        Task<string> GetSecurityStampAsync(string email, CancellationToken token);

        Task<UserDto> GetUserByNameAsync(string username, CancellationToken cancellationToken);
        Task<UserDto> GetUserByEmailAsync(string Email, CancellationToken cancellationToken);
        Task<UserDto> GetUserByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<LikeDto[]> GetUserLikesAsync(string userName, CancellationToken token = default);
        Task<UserDto[]> GetAllUsers(CancellationToken token);
        Task<UserTokenDto> GetUserDataByRefreshTokenAsync(Guid id, CancellationToken cancellationToken);
    }
}
