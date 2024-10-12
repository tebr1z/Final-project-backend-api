using LmsApiApp.Application.Dtos.UserDtos;
using LmsApiApp.Application.Interfaces;
using LmsApiApp.Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LmsApiApp.Application.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthService(UserManager<User> userManager,
                           SignInManager<User> signInManager,
                           RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterDto registerDto)
        {
            var user = new User
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
                // Diğer gerekli alanları doldurabilirsiniz
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded && !string.IsNullOrEmpty(registerDto.Role))
            {
                // Rolün var olup olmadığını kontrol et
                if (await _roleManager.RoleExistsAsync(registerDto.Role))
                {
                    await _userManager.AddToRoleAsync(user, registerDto.Role);
                }
                else
                {
                    // Rol mevcut değilse hata dönebilirsiniz veya yeni rol oluşturabilirsiniz
                    // Örneğin, yeni rol oluşturmak:
                    await _roleManager.CreateAsync(new IdentityRole(registerDto.Role));
                    await _userManager.AddToRoleAsync(user, registerDto.Role);
                }
            }

            return result;
        }

        public async Task<SignInResult> LoginAsync(LoginDto loginDto)
        {
            var result = await _signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, isPersistent: false, lockoutOnFailure: false);
            return result;
        }

        public async Task<IdentityResult> AssignRoleAsync(UserRoleDto userRoleDto)
        {
            var user = await _userManager.FindByIdAsync(userRoleDto.UserId);
            if (user == null)
            {
                var identityError = new IdentityError
                {
                    Description = "Kullanıcı bulunamadı."
                };
                return IdentityResult.Failed(identityError);
            }

            if (!await _roleManager.RoleExistsAsync(userRoleDto.Role))
            {
                var identityError = new IdentityError
                {
                    Description = "Rol mevcut değil."
                };
                return IdentityResult.Failed(identityError);
            }

            var result = await _userManager.AddToRoleAsync(user, userRoleDto.Role);
            return result;
        }
    }
}
