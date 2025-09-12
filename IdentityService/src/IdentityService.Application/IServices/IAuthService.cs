using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityService.Application.Dtos.Auth;

namespace IdentityService.Application.IServices
{
    public interface IAuthService
    {
        Task<LoginResDto> LoginAsync(LoginReqDto loginReqDto);
        Task<RegisterResDto> RegisterAsync(RegisterReqDto registerReqDto);
        Task<ForgetPasswordResDto> ForgetPasswordAsync(ForgetPasswordReqDto forgetPasswordReqDto);
        Task<VerifyOtpResDto> VerifyOtpAsync(VerifyOtpReqDto verifyOtpReqDto);
        Task ResetPasswordAsync(ResetPasswordReqDto resetPasswordReqDto);

    }
}