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
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Events;

namespace IdentityService.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordResetOtpRepository _passwordResetOtpRepository;
        private readonly IResetTokenRepository _resetTokenRepository;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<AuthService> _logger;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;
        private const int OtpLength = 5;
        private const int MaxOtpAttempts = 3;
        private const int OtpExpiryMinutes = 10;
        private const int ResetTokenExpiryMinutes = 15;
        public AuthService(
            IUserRepository userRepository,
            IPasswordResetOtpRepository passwordResetOtpRepository,
            IResetTokenRepository resetTokenRepository,
            IJwtGenerator jwtGenerator,
            IPasswordHasher passwordHasher,
            ILogger<AuthService> logger,
            IPublishEndpoint publishEndpoint,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _passwordResetOtpRepository = passwordResetOtpRepository;
            _resetTokenRepository = resetTokenRepository;
            _passwordHasher = passwordHasher;
            _jwtGenerator = jwtGenerator;
            _logger = logger;
            _publishEndpoint = publishEndpoint;
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
                if (!user.IsActive)
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

        public async Task<ForgetPasswordResDto> ForgetPasswordAsync(ForgetPasswordReqDto forgetPasswordReqDto)
        {
            try
            {
                _logger.LogInformation("Processing forget password request for email: {Email}", forgetPasswordReqDto.Email);
                var user = await _userRepository.GetSingleAsync(u => u.Email == forgetPasswordReqDto.Email)
                    ?? throw new NotFoundException(forgetPasswordReqDto.Email, "User");
                 if (!user.IsActive)
                {
                    throw new InactiveUserException(user.Id);
                }

                var otp = GenerateOTP();
                var passwordResetOtp = new PasswordResetOtp
                {
                    UserId = user.Id,
                    Otp = otp,
                    ExpiryTime = DateTime.UtcNow.AddMinutes(OtpExpiryMinutes),
                };
                await _passwordResetOtpRepository.AddAsync(passwordResetOtp);
                await _publishEndpoint.Publish<ForgotPasswordEvent>(new
                {
                    Email = forgetPasswordReqDto.Email,
                    OTP = otp
                });

                _logger.LogInformation("Password reset link sent to email: {Email}", forgetPasswordReqDto.Email);
                var result = new ForgetPasswordResDto { OtpId = passwordResetOtp.Id };
                return result;
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning("Forget password request failed for email: {Email}. Reason: {Reason}", forgetPasswordReqDto.Email, ex.Message);
                throw;
            }
            catch (InactiveUserException ex)
            {
                _logger.LogWarning("Inactive user forget password attempt for user: {Email}. Reason: {Reason}", forgetPasswordReqDto.Email, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing forget password for email: {Email}", forgetPasswordReqDto.Email);
                throw;
            }
        }

        public async Task<VerifyOtpResDto> VerifyOtpAsync(VerifyOtpReqDto verifyOtpReqDto)
        {
            try
            {
                _logger.LogInformation("Verifying OTP for OtpId: {OtpId}", verifyOtpReqDto.OtpId);
                var passwordResetOtp = await _passwordResetOtpRepository.GetSingleAsync(o => o.Id == verifyOtpReqDto.OtpId)
                    ?? throw new NotFoundException(verifyOtpReqDto.OtpId, "OTP");

                if (passwordResetOtp.IsUsed)
                {
                    throw new Exceptions.InvalidOperationException("OTP has already been used.");
                }

                if (passwordResetOtp.ExpiryTime < DateTime.UtcNow)
                {
                    throw new Exceptions.InvalidOperationException("OTP has expired.");
                }

                if (passwordResetOtp.Otp != verifyOtpReqDto.Otp)
                {
                    passwordResetOtp.AttemptCount++;
                    if (passwordResetOtp.AttemptCount >= MaxOtpAttempts)
                    {
                        passwordResetOtp.IsUsed = true;
                    }
                    await _passwordResetOtpRepository.UpdateAsync(passwordResetOtp);
                    throw new Exceptions.InvalidOperationException("Invalid OTP.");
                }

                passwordResetOtp.IsUsed = true;
                await _passwordResetOtpRepository.UpdateAsync(passwordResetOtp);
                var resetTokenEntity = new ResetToken
                {
                    UserId = passwordResetOtp.UserId,
                    ExpiryTime = DateTime.UtcNow.AddMinutes(ResetTokenExpiryMinutes)
                };
                await _resetTokenRepository.AddAsync(resetTokenEntity);
                var result = new VerifyOtpResDto
                {
                    IsVerified = true,
                    ResetToken = resetTokenEntity.Id
                };
                _logger.LogInformation("OTP verified successfully for OtpId: {OtpId}", verifyOtpReqDto.OtpId);
                return result;
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning("OTP verification failed for OtpId: {OtpId}. Reason: {Reason}", verifyOtpReqDto.OtpId, ex.Message);
                throw;
            }
            catch (Exceptions.InvalidOperationException ex)
            {
                _logger.LogWarning("OTP verification failed for OtpId: {OtpId}. Reason: {Reason}", verifyOtpReqDto.OtpId, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while verifying OTP for OtpId: {OtpId}", verifyOtpReqDto.OtpId);
                throw;
            }
        }

        public async Task ResetPasswordAsync(ResetPasswordReqDto resetPasswordReqDto)
        {
            try
            {
                _logger.LogInformation("Resetting password using reset token.");
                var resetTokenEntity = await _resetTokenRepository.GetSingleAsync(rt => rt.Id == resetPasswordReqDto.ResetTokenId)
                    ?? throw new Exceptions.InvalidOperationException("Invalid reset token.");

                if (resetTokenEntity.ExpiryTime < DateTime.UtcNow)
                {
                    throw new Exceptions.InvalidOperationException("Reset token has expired.");
                }

                var user = await _userRepository.GetByIdAsync(resetTokenEntity.UserId)
                    ?? throw new NotFoundException(resetTokenEntity.UserId, "User");

                user.PasswordHash = _passwordHasher.HashPassword(resetPasswordReqDto.NewPassword);
                await _userRepository.UpdateAsync(user);
                await _resetTokenRepository.DeleteAsync(resetTokenEntity.Id);

                _logger.LogInformation("Password reset successfully for user: {Username}", user.Username);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning("Password reset failed. Reason: {Reason}", ex.Message);
                throw;
            }
            catch (Exceptions.InvalidOperationException ex)
            {
                _logger.LogWarning("Password reset failed. Reason: {Reason}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while resetting password.");
                throw;
            }
        }
      
        private string GenerateOTP()
        {
            string otp = string.Empty;
            for (int i = 0; i < OtpLength; i++)
            {
                var x = new Random().Next(0, 10).ToString();
                otp += x;
            }

            return otp;
        }
        
    }
}