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
        
    }
}