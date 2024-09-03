using NewsAggregatorDTOs;
using NewsAggregatorMVCModels;

namespace NewsAggregatorApp.Services.Abstractions
{
    public interface IUserService
    {
        Task RegisterUserAsync(string name, string email, string password, CancellationToken token);
        Task<bool> CheckPasswordAsync(string email, string password);
        Task<bool> CheckIsEmailRegisteredAsync(string email, CancellationToken token);
        //Task<string> GetUserRoleNameByIdAsync(Guid id, CancellationToken token);
        Task<string> GetUserRoleNameByEmailAsync(string modelEmail, CancellationToken token);
        Task<string> GetUserNameByEmailAsync(string name, CancellationToken token);
        Task<LikeDto[]> GetUserLikesAsync(string userName);
        Task AddCommentAsync(CommentModel model, string name);
        Task<UserDto[]> GetAllUsers();
        Task ChangeUserRoleAsync(Guid id);
		//Task<string> GetUserRoleByNameAsync(string modelName, CancellationToken token);
		//Task<Guid?> GetUserIdByEmailAsync(string modelEmail, CancellationToken cancellationToken);



		//Task<UserTokenDto> GetUserDataByRefreshToken(Guid id, CancellationToken cancellationToken);
	}
}
