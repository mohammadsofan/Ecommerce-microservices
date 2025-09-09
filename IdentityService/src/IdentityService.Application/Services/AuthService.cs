using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using IdentityService.Application.Dtos.Auth;
using IdentityService.Application.Exceptions;
using IdentityService.Application.IAuth;
using IdentityService.Application.IRepository;
using IdentityService.Application.IServices;
using IdentityService.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<AuthService> _logger;
        private readonly IMapper _mapper;
        public AuthService(
            IUserRepository userRepository,
            IJwtGenerator jwtGenerator,
            IPasswordHasher passwordHasher,
            ILogger<AuthService> logger,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _jwtGenerator = jwtGenerator;
            _passwordHasher = passwordHasher;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<LoginResDto> LoginAsync(LoginReqDto loginReqDto)
        {
            try
            {
                _logger.LogInformation("Attempting to log in user: {Username}", loginReqDto.Username);
                var user = await _userRepository.GetSingleAsync(u => u.Username == loginReqDto.Username)
                    ?? throw new InvalidCredentialsException();
                
                var validPassword = _passwordHasher.VerifyPassword(loginReqDto.Password, user.PasswordHash);
                if (!validPassword)
                {
                    throw new InvalidCredentialsException();
                }
                if(!user.IsActive)
                {
                    throw new InactiveUserException(user.Id);
                }
                var token = _jwtGenerator.GenerateJwtToken(user);
                _logger.LogInformation("User logged in successfully: {Username}", loginReqDto.Username);

                return new LoginResDto { Token = token };
            }
            catch (InvalidCredentialsException)
            {
                _logger.LogWarning("Invalid login attempt for user: {Username}", loginReqDto.Username);
                throw;
            }
            catch (InactiveUserException ex)
            {
                _logger.LogWarning("Inactive user login attempt for user: {Username}. Reason: {Reason}", loginReqDto.Username, ex.Message);
                throw;
            }
            catch (Exception)
            {
                _logger.LogError("An error occurred during login for user: {Username}", loginReqDto.Username);

                throw;
            }
        }

        public async Task<RegisterResDto> RegisterAsync(RegisterReqDto registerReqDto)
        {
            try
            {
                _logger.LogInformation("Attempting to register user: {Username}", registerReqDto.Username);
                var alredyExists = _userRepository.ExistsAsync(u => u.Username == registerReqDto.Username || u.Email == registerReqDto.Email);
                if (alredyExists.Result)
                {
                    throw new Exceptions.InvalidOperationException("Username or Email already in use.");
                }
                var hashedPassword = _passwordHasher.HashPassword(registerReqDto.Password);
                var user = _mapper.Map<User>(registerReqDto);
                user.PasswordHash = hashedPassword;

                await _userRepository.AddAsync(user);
                _logger.LogInformation("User registered successfully: {Username}", registerReqDto.Username);
                return _mapper.Map<RegisterResDto>(user);

            }
            catch (Exceptions.InvalidOperationException ex)
            {
                _logger.LogWarning("Registration attempt failed for user: {Username}. Reason: {Reason}", registerReqDto.Username, ex.Message);
                throw;
            }
            catch (Exception)
            {
                _logger.LogError("An error occurred during registration for user: {Username}", registerReqDto.Username);
                throw;
            }
        }
    }
}