using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityService.Api.Extensions;
using IdentityService.Application.Dtos.Address;
using IdentityService.Application.Dtos.User;
using IdentityService.Application.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Api.Controllers
{
    [ApiController]
    [Route("api/identity/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserDto>> GetUserByIdAsync(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            return Ok(user);
        }
        [HttpDelete()]
        public async Task<ActionResult> DeleteUserAsync()
        {
            var userId = User.GetUserId();
            await _userService.DeleteUserAsync(userId);
            return NoContent();
        }
        [HttpGet("profile")]
        public async Task<ActionResult<UserDto>> GetUserProfileAsync()
        {
            var userId = User.GetUserId();
            var user = await _userService.GetUserByIdAsync(userId);
            return Ok(user);
        }
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<UserDto>>> GetAllUsersAsync([FromQuery] FilterUsersDto filterUsersDto)
        {
            var users = await _userService.GetAllUsersAsync(filterUsersDto);
            return Ok(users);
        }
        [HttpPatch()]
        public async Task<ActionResult> PatchUserPersonalInfoAsync([FromBody] UpdateUserPersonalInfoDto updateUserDto)
        {
            var userId = User.GetUserId();
            await _userService.PatchUserPersonalInfoAsync(userId, updateUserDto);
            return NoContent();
        }
        [HttpDelete("address/{addressId}")]
        public async Task<ActionResult> DeleteUserAddressAsync(string addressId)
        {
            var userId = User.GetUserId();
            await _userService.RemoveAddressFromUserAsync(userId, addressId);
            return NoContent();
        }
        [HttpPost("address")]
        public async Task<ActionResult> AddUserAddressAsync([FromBody] CreateAddressDto addressDto)
        {
            var userId = User.GetUserId();
            await _userService.AddAddressToUserAsync(userId, addressDto);
            return NoContent();
        }
        [HttpPatch("address/{addressId}")]
        public async Task<ActionResult> PatchUserAddressAsync( string addressId, [FromBody] UpdateAddressDto addressDto)
        {
            var userId = User.GetUserId();
            await _userService.PatchUserAddressAsync(userId, addressId, addressDto);
            return NoContent();
        }

    }
}