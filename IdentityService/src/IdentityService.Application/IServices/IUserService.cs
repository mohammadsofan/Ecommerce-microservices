using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityService.Application.Dtos.Address;
using IdentityService.Application.Dtos.User;

namespace IdentityService.Application.IServices
{
    public interface IUserService
    {
        Task<UserDto> GetUserByIdAsync(string userId);
        Task<IEnumerable<UserDto>> GetAllUsersAsync(FilterUsersDto filterDto);
        Task<bool> DeleteUserAsync(string userId);
        Task<UserDto> PatchUserPersonalInfoAsync(string userId, UpdateUserPersonalInfoDto userDto);
        Task<UserDto> AddAddressToUserAsync(string userId, CreateAddressDto createAddressDto);
        Task<bool> RemoveAddressFromUserAsync(string userId, string addressId);
        Task<UserDto> PatchUserAddressAsync(string userId, string addressId, UpdateAddressDto updateAddressDto);
    }
}