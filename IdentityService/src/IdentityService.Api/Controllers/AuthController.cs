using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityService.Application.Dtos.Auth;
using IdentityService.Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Api.Controllers
{
    [ApiController]
    [Route("api/identity/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("login")]
        public async Task<ActionResult<LoginResDto>> LoginAsync([FromBody] LoginReqDto loginReqDto)
        {
            var result = await _authService.LoginAsync(loginReqDto);
            return Ok(result);
        }
        [HttpPost("register")]
        public async Task<ActionResult<RegisterResDto>> RegisterAsync([FromBody] RegisterReqDto registerReqDto)
        {
            var result = await _authService.RegisterAsync(registerReqDto);
            return Ok(result);
        }
        [HttpPost("forget-password")]
        public async Task<ActionResult<ForgetPasswordResDto>> ForgetPasswordAsync([FromBody] ForgetPasswordReqDto forgetPasswordReqDto)
        {
            var result = await _authService.ForgetPasswordAsync(forgetPasswordReqDto);
            return Ok(result);
        }
        [HttpPost("verify-otp")]
        public async Task<ActionResult<VerifyOtpResDto>> VerifyOtpAsync([FromBody] VerifyOtpReqDto verifyOtpReqDto)
        {
            var result = await _authService.VerifyOtpAsync(verifyOtpReqDto);
            return Ok(result);
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordReqDto resetPasswordReqDto)
        {
            await _authService.ResetPasswordAsync(resetPasswordReqDto);
            return NoContent();
        }

        
    }
}