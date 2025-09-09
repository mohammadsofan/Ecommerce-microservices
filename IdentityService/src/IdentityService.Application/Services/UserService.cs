using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using IdentityService.Application.Dtos.Address;
using IdentityService.Application.Dtos.User;
using IdentityService.Application.Exceptions;
using IdentityService.Application.IRepository;
using IdentityService.Application.IServices;
using IdentityService.Domain.Entities;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace IdentityService.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(
            IUserRepository userRepository,
            IMapper mapper,
            ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UserDto> AddAddressToUserAsync(string userId, CreateAddressDto createAddressDto)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId)
                    ?? throw new NotFoundException(userId, "User");
                if (!user.IsActive)
                {
                    throw new InactiveUserException(userId);
                }
                
                var address = _mapper.Map<Address>(createAddressDto);
                user.AddAddress(address);
                await _userRepository.UpdateAsync(user);
                return _mapper.Map<UserDto>(user);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning("Add address failed for user with ID: {UserId}: {ExceptionMessage}", userId, ex.Message);
                throw;
            }
            catch (InactiveUserException ex)
            {
                _logger.LogWarning("Add address failed for user with ID: {UserId}: {ExceptionMessage}", userId, ex.Message);
                throw;
            }
            catch (System.InvalidOperationException ex)
            {
                _logger.LogWarning("Add address failed for user with ID: {UserId}: {ExceptionMessage}", userId, ex.Message);
                throw new Exceptions.InvalidOperationException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding address to user with ID: {UserId}", userId);
                throw;
            }
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            try
            {
                _logger.LogInformation("Attempting to delete user with ID: {UserId}", userId);
                var user = await _userRepository.GetByIdAsync(userId);
                if (user is null)
                {
                    _logger.LogWarning("User with ID: {UserId} not found", userId);
                    return false;
                }
                if (!user.IsActive)
                {
                    _logger.LogWarning("User with ID: {UserId} is already inactive", userId);
                    return false;
                }
                user.IsActive = false;
                await _userRepository.UpdateAsync(user);
                _logger.LogInformation("User with ID: {UserId} soft deleted successfully", userId);
                return true;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user with ID: {UserId}", userId);
                throw;
            }
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync(FilterUsersDto filterDto)
        {
            try
            {
                _logger.LogInformation("Fetching all users with applied filters: {@FilterDto}", filterDto);
                var builder = Builders<User>.Filter;
                var filters = new List<FilterDefinition<User>>();
                GetFilters(filterDto, filters);
                var combinedFilter = filters.Any() ? builder.And(filters) : builder.Empty;

                var users = await _userRepository.FindAsync(combinedFilter);

                var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);

                _logger.LogInformation("Fetched {Count} users with applied filters {@FilterDto}", userDtos.Count(), filterDto);
                return userDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching users with filters: {@FilterDto}", filterDto);
                throw;
            }
        }

        private static void GetFilters(FilterUsersDto filterDto, List<FilterDefinition<User>> filters)
        {
            if (filterDto.City is not null)
                filters.Add(Builders<User>.Filter.ElemMatch(u => u.Addresses,
                    Builders<Address>.Filter.Regex(a => a.City, new MongoDB.Bson.BsonRegularExpression($"^{filterDto.City}$", "i"))));

            if (filterDto.State is not null)
                filters.Add(Builders<User>.Filter.ElemMatch(u => u.Addresses,
                    Builders<Address>.Filter.Regex(a => a.State, new MongoDB.Bson.BsonRegularExpression($"^{filterDto.State}$", "i"))));

            if (filterDto.Country is not null)
                filters.Add(Builders<User>.Filter.ElemMatch(u => u.Addresses,
                    Builders<Address>.Filter.Regex(a => a.Country, new MongoDB.Bson.BsonRegularExpression($"^{filterDto.Country}$", "i"))));

            if (filterDto.IsActive.HasValue)
                filters.Add(Builders<User>.Filter.Eq(u => u.IsActive, filterDto.IsActive.Value));

            if (filterDto.Username is not null)
                filters.Add(Builders<User>.Filter.Regex(u => u.Username,
                    new MongoDB.Bson.BsonRegularExpression($"^{filterDto.Username.ToLower()}$", "i")));

            if (filterDto.FullName is not null)
                filters.Add(Builders<User>.Filter.Regex(u => u.FullName,
                    new MongoDB.Bson.BsonRegularExpression($"^{filterDto.FullName.ToLower()}$", "i")));
        }

        public async Task<UserDto> GetUserByIdAsync(string userId)
        {
            try
            {
                _logger.LogInformation("Fetching user with ID: {UserId}", userId);
                var user = await _userRepository.GetByIdAsync(userId)
                    ?? throw new NotFoundException(userId, "User");
                
                if(!user.IsActive)
                {
                    throw new InactiveUserException(userId);
                }

                var userDto = _mapper.Map<UserDto>(user);
                _logger.LogInformation("User with ID: {UserId} fetched successfully", userId);

                return userDto;
                
            }
            catch (InactiveUserException ex)
            {
                _logger.LogWarning("Get user failed: {ExceptionMessage}", ex.Message);
                throw;
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning("Get user failed: {ExceptionMessage}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching user with ID: {UserId}", userId);
                throw;
            }
        }

        public async Task<bool> RemoveAddressFromUserAsync(string userId, string addressId)
        {
            try
            {
                _logger.LogInformation("Attempting to remove address with ID: {AddressId} from user with ID: {UserId}", addressId, userId);
                var user = await _userRepository.GetByIdAsync(userId)
                    ?? throw new NotFoundException(userId, "User");
                if (!user.IsActive)
                {
                    throw new InactiveUserException(userId);
                }
                user.RemoveAddress(addressId);
                await _userRepository.UpdateAsync(user);
                _logger.LogInformation("Address with ID: {AddressId} removed from user with ID: {UserId} successfully", addressId, userId);
                return true;
                

            }
            catch (InactiveUserException ex)
            {
                _logger.LogWarning("Removing address failed for user with ID: {UserId}: {ExceptionMessage}", userId, ex.Message);
                throw;
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning("Removing address failed for user with ID: {UserId}: {ExceptionMessage}", userId, ex.Message);
                throw;
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Removing address failed for user with ID: {UserId}: {ExceptionMessage}", userId, ex.Message);
                throw new NotFoundException(addressId, "Address");
            }
            catch (System.InvalidOperationException ex)
            {
                _logger.LogWarning("Removing address failed for user with ID: {UserId}: {ExceptionMessage}", userId, ex.Message);
                throw new Exceptions.InvalidOperationException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing address with ID: {AddressId} from user with ID: {UserId}", addressId, userId);
                throw;
            }
        }

        public async Task<UserDto> PatchUserAddressAsync(string userId, string addressId, UpdateAddressDto updateAddressDto)
        {
            try
            {
                _logger.LogInformation("Attempting to update address with ID: {AddressId} for user with ID: {UserId}", addressId, userId);
                var user = await _userRepository.GetByIdAsync(userId)
                    ?? throw new NotFoundException(userId, "User");
                if (!user.IsActive)
                {
                    throw new InactiveUserException(userId);
                }
                var address = user.Addresses.FirstOrDefault(a => a.Id == addressId)
                    ?? throw new NotFoundException(addressId, "Address");

                var updatedAddress = _mapper.Map<Address>(updateAddressDto);
                _mapper.Map(updateAddressDto, address);
                await _userRepository.UpdateAsync(user);
                _logger.LogInformation("Address with ID: {AddressId} for user with ID: {UserId} updated successfully", addressId, userId);
                return _mapper.Map<UserDto>(user);

            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning("Update address failed for user with ID: {UserId}: {ExceptionMessage}", userId, ex.Message);
                throw;
            }
            catch (InactiveUserException ex)
            {
                _logger.LogWarning("Update address failed for user with ID: {UserId}: {ExceptionMessage}", userId, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating address with ID: {AddressId} for user with ID: {UserId}", addressId, userId);
                throw;
            }
        }

        public async Task<UserDto> PatchUserPersonalInfoAsync(string userId, UpdateUserPersonalInfoDto userDto)
        {
            try
            {
                _logger.LogInformation("Attempting to update personal info for user with ID: {UserId}", userId);
                var user = await _userRepository.GetByIdAsync(userId)
                    ?? throw new NotFoundException(userId, "User");
                if (!user.IsActive)
                {
                    throw new InactiveUserException(userId);
                }
                var updatedUser = _mapper.Map<User>(userDto);
                _mapper.Map(userDto, user);

                await _userRepository.UpdateAsync(user);
                _logger.LogInformation("Personal info for user with ID: {UserId} updated successfully", userId);
                return _mapper.Map<UserDto>(user);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning("Update user personal info failed: {ExceptionMessage}", ex.Message);
                throw;
            }
            catch (InactiveUserException ex)
            {
                _logger.LogWarning("Update user personal info failed: {ExceptionMessage}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating personal info for user with ID: {UserId}", userId);
                throw;
            }
        }
    }
}