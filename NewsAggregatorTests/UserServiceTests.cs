using NewsAggregatorApp.Entities;
using NewsAggregatorApp.Mappers;
using NewsAggregatorApp.Services.Abstractions;
using NewsAggregatorApp.Services;

using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using NewsAggregatorDTOs;
using NewsAggregatorCQS.Queries.User;
using NewsAggregatorCQS.Commands.User;
using NewsAggregatorCQS.Queries.Categories;

namespace NewsAggregatorTests
{
	public class UserServiceTests
	{
		private IMediator _mediatorMock;
		private ILogger<UserService> _loggerMock;

		private string resultRole = "";

		private List<UserDto> _users = []; //is it ok to define list of DTO's in tests?

		private UserService GetUserServiceWithMocks(Guid userId, string newRole)
		{
			_users =
			[
			   new UserDto
			   {
				   UserDtoId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
				   RoleId = Guid.Parse("AA52B01B-3999-4473-912B-1BF84F13B012"),
				   RoleName = "Admin",
				   Name = "test admin",
				   Email = ""
			   },
				new UserDto
				{
					UserDtoId = Guid.Parse("21111111-1111-1111-1111-111111111111"),
					RoleId = Guid.Parse("1FD82CBA-0198-4520-8988-C147C9AD70BE"),
					RoleName = "User",
					Name = "test user",
					Email = ""
				}
			];
			var cToken = new CancellationToken();

			_mediatorMock = Substitute.For<IMediator>();
			_loggerMock = Substitute.For<ILogger<UserService>>();

			_mediatorMock.Send(new GetUsersQuery())
				.ReturnsForAnyArgs(Task.FromResult(_users.ToArray()));

			//_mediatorMock.Send(new SetRoleCommand() { UserId = userId, RoleName = Arg.Any<string>() })
			//	.ReturnsForAnyArgs(Task.FromResult(1))
			//	.AndDoes(info =>
			//	{
			//		resultRole = (info.Args())[0].ToString();
			//	});


			_mediatorMock.Send(new SetRoleCommand() { UserId = userId, RoleName = Arg.Any<string>() })
				.ReturnsForAnyArgs(Task.FromResult(1));


			return new UserService(_mediatorMock, _loggerMock);
		}

		[Theory]
		[InlineData("11111111-1111-1111-1111-111111111111")]
		[InlineData("21111111-1111-1111-1111-111111111111")]

		public async Task ChangeUserRoleAsync_ChangeWithCorrectId_ChangedCorrectly(string guidValue)
		{
			//Arrange
			var userId = Guid.Parse(guidValue);
			string newRole = "User";
			//Act
			var userService = GetUserServiceWithMocks(userId, newRole);
			//Assert
			await userService.ChangeUserRoleAsync(userId);
			//_mediatorMock.Received().SetRoleCommand();
			Assert.Equal(newRole, resultRole);
		}

		[Theory]
		[InlineData("11111111-1111-1111-1111-111111111111")]
		[InlineData("21111111-1111-1111-1111-111111111111")]

		public async Task ChangeUserRoleAsync_ChangeWithInorrectId_ThrowException(string guidValue)
		{
			//Arrange
			var userId = Guid.Parse(guidValue);
			string newRole = "User";
			//Act
			var userService = GetUserServiceWithMocks(userId, newRole);
			//Assert
			await userService.ChangeUserRoleAsync(userId);
			Assert.Equal(newRole, resultRole);
		}
	}
}
