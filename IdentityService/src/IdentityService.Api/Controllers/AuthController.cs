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

        
    }
}