using LmsApiApp.Application.Dtos.UserDtos;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LmsApiApp.Application.Interfaces
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterAsync(RegisterDto registerDto);
        Task<SignInResult> LoginAsync(LoginDto loginDto);
        Task<IdentityResult> AssignRoleAsync(UserRoleDto userRoleDto);
    }
}
