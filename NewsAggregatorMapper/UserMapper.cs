using NewsAggregatorApp.Entities;
using NewsAggregatorDTOs;
using NewsAggregatorMVCModels;
using Riok.Mapperly.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorMapper
{
    [Mapper]
    public static partial class UserMapper
    {
        [MapProperty(nameof(User.UserId), nameof(UserDto.UserDtoId))]
        [MapProperty([nameof(User.Role), nameof(User.Role.RoleName)], [nameof(UserDto.RoleName)])]
		public static partial UserDto? UserToUserDto(User? user);

        [MapProperty(nameof(UserDto.UserDtoId), nameof(User.UserId))]
        public static partial User? UserDtoToUser(UserDto? userDto);

        [MapProperty(nameof(RegisterUserDto.RegisterUserDtoId), nameof(User.UserId))]
        public static partial User? RegisterUserDtoToUser(RegisterUserDto? registerUserDto);

        [MapProperty(nameof(UserDto.UserDtoId), nameof(UserModel.UserModelId))]
        public static partial UserModel? UserDtoToUserModel(UserDto? userDto);

        [MapProperty(nameof(UserModel.UserModelId), nameof(UserDto.UserDtoId))]
        public static partial UserDto? UserModelToUserDto(UserModel? userDto);

    }
}
