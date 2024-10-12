using LmsApiApp.Application.Dtos.UserDtos;
using LmsApiApp.Application.Interfaces;
using LmsApiApp.Application.Settings;
using LmsApiApp.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Web;

namespace LmsApiApp.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;
        private readonly JwtSetting _jwtSetting;
        private readonly IEmailService _emailService;
        private readonly TimeSpan otpExpiryDuration = TimeSpan.FromMinutes(10);

        public AuthController(
            IOptions<JwtSetting> jwtsetting,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            ITokenService tokenService,
            IEmailService emailService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _jwtSetting = jwtsetting.Value;
            _emailService = emailService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingUser = await _userManager.FindByNameAsync(registerDto.UserName);
            if (existingUser != null)
                return Conflict("Kullanıcı adı zaten mevcut.");

            var userByEmail = await _userManager.FindByEmailAsync(registerDto.Email);
            if (userByEmail != null)
                return Conflict("Bu e-posta adresi zaten kullanılıyor.");

            var user = new User
            {
                UserName = registerDto.UserName,
                FullName = registerDto.UserName, // FullName yerine registerDto.FullName kullanabilirsiniz
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // "User" rolünü ekleyin, rol mevcut değilse oluşturabilirsiniz
            if (!await _roleManager.RoleExistsAsync("User"))
                await _roleManager.CreateAsync(new IdentityRole("User"));

            await _userManager.AddToRoleAsync(user, "User");

            // E-posta doğrulama token'ı oluşturun
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var link = Url.Action(nameof(VerifyEmail), "Auth", new { email = user.Email, token }, Request.Scheme, Request.Host.ToString());

            // E-posta şablonunu okuyun ve link ile değiştirin
            string body;
            using (var stream = new StreamReader("wwwroot/template/verifyEmailTemplate.html"))
            {
                body = await stream.ReadToEndAsync();
            }
            body = body.Replace("{{link}}", link);
            body = body.Replace("{{username}}", user.UserName);

            _emailService.SendEmail(new List<string> { user.Email }, "E-posta Doğrulama", body);

            return Ok("Kayıt başarılı. Lütfen e-posta adresinizi doğrulayın.");
        }

        [HttpGet("verifyEmail")]
        public async Task<IActionResult> VerifyEmail(string token, string email)
        {
            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(email))
                return BadRequest("Token ve email gerekli.");

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound("Kullanıcı bulunamadı.");

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
                return Ok("E-posta doğrulandı.");

            return BadRequest("E-posta doğrulama başarısız.");
        }

        // Diğer metodlar (Login, ResetPassword, OTP gönderimi vs.) burada kalacak.
        // Orijinal kodunuzdaki metodları kullanabilirsiniz.
    }
}
