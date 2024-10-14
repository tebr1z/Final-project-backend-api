using LmsApiApp.Application.Dtos.UserDtos;
using LmsApiApp.Application.Interfaces;
using LmsApiApp.Application.Settings;
using LmsApiApp.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
namespace LmsApiApp.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;
        private readonly JwtSetting _jwtSetting;
        private readonly IEmailService _emailService;
        private readonly TimeSpan otpExpiryDuration = TimeSpan.FromMinutes(10);
        public AuthController(IOptions<JwtSetting> jwtsetting, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ITokenService tokenService, IEmailService emailService, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _jwtSetting = jwtsetting.Value;
            _emailService = emailService;
            _signInManager = signInManager;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registeDto)
        {
            if (!ModelState.IsValid)
            {

                return BadRequest(ModelState);
            }
         

            User user = await _userManager.FindByNameAsync(registeDto.UserName);
            if (user != null) return Conflict();
            user = new()
            {
                UserName = registeDto.UserName,
                FullName = registeDto.FullName,
                LastName = registeDto.LastName,
              
                Email = registeDto.Email,
                Img = registeDto.Img ?? "default.png", // Varsayılan resim
                IsDeleted = false,
                IsBanned = false,
              
            };
           
            if (string.IsNullOrEmpty(registeDto.UserName))
            {
                // Logla veya hata mesajı fırlat
                Console.WriteLine($"UserName: {user.UserName}");
            }
            try
            {
                IdentityResult result = await _userManager.CreateAsync(user, registeDto.Password);
                if (!result.Succeeded)
                    return BadRequest(result.Errors);

                await _userManager.AddToRoleAsync(user, "Student");
            }
            catch (Exception ex)
            {
                // Hata mesajını ve içsel hatayı konsola yazdır
                Console.WriteLine($"Hata: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"İçsel Hata: {ex.InnerException.Message}");
                }
                return StatusCode(500, "Sunucu hatası. Lütfen tekrar deneyin.");
            }






            string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            string link = Url.Action(nameof(VerifyEamil), "Auth", new { email = user.Email, token = token },
                Request.Scheme, Request.Host.ToString());

            string body = string.Empty;
            using (StreamReader stream = new StreamReader("wwwroot/template/verifyEmailTemplate.html"))
            {
                body = stream.ReadToEnd();
            }
            body = body.Replace("{{link}}", link);
            body = body.Replace("{{username}}", user.UserName);

            _emailService.SendEmail(new List<string>() { user.Email }, "verify Email", body);



            return Ok();
        }



        [HttpGet("verifyEmail")]



        public async Task<IActionResult> VerifyEamil(string token, string email)
        {
            User user = await _userManager.FindByEmailAsync(email);
            if (user == null) return NotFound();
            await _userManager.ConfirmEmailAsync(user, token);

            return Ok("Email confirim");
        }


















        [HttpPost("resetPasswordSendOtp")]
        public async Task<IActionResult> SendResetPasswordOtp(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return NotFound();


            Random random = new Random();
            string otpCode = random.Next(000001, 999999).ToString();

            user.ResetPasswordOtp = otpCode;
            user.OtpExpiryTime = DateTime.UtcNow.AddMinutes(10);
            await _userManager.UpdateAsync(user);

            // OTP'yi içeren bir e-posta gönder
            string body = string.Empty;
            using (StreamReader stream = new StreamReader("wwwroot/template/otpTemplate.html"))
            {
                body = stream.ReadToEnd();
            }
            body = body.Replace("{{otpCode}}", otpCode);
            body = body.Replace("{{username}}", user.UserName);

            _emailService.SendEmail(new List<string>() { user.Email }, "Your OTP Code", body);

            return Ok("OTP kodu e-poçta adresinə göndərildi");
        }




        [HttpPost("resetPasswordWithOtp")]
        public async Task<IActionResult> ResetPasswordWithOtp(string email, string otpCode, [FromBody] ResetPasswordDto resetPasswordDto)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(otpCode))
                return BadRequest(new { error = "Email və otp mütləqdiq!" });

            if (resetPasswordDto.Password != resetPasswordDto.RePassword)
                return BadRequest("Kod uygunlaşmır ");

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest("User Not Found");


            if (user.ResetPasswordOtp != otpCode || user.OtpExpiryTime < DateTime.UtcNow)
                return BadRequest("Geçərsiz Ve ya Vaxtı dolmuş Otp Kod");

            // Şifre sıfırlama
            var result = await _userManager.RemovePasswordAsync(user);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            result = await _userManager.AddPasswordAsync(user, resetPasswordDto.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // OTP ve süresini temizle
            user.ResetPasswordOtp = null;
            user.OtpExpiryTime = null;
            await _userManager.UpdateAsync(user);

            return Ok("Şirə dəyişdi");
        }


        [HttpPost("verifyOtp")]
        public async Task<IActionResult> VerifyOtp(string email, string otpCode)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound(new { error = "User not found" });
            }

            if (string.IsNullOrEmpty(user.ResetPasswordOtp) || user.OtpExpiryTime == null)
            {
                return BadRequest(new { error = "OTP has not been set or is invalid" });
            }

            if (user.ResetPasswordOtp != otpCode)
            {
                return BadRequest(new { error = "Invalid OTP code" });
            }

            if (DateTime.UtcNow > user.OtpExpiryTime)
            {
                return BadRequest(new { error = "OTP has expired" });
            }

            return Ok(new { message = "OTP is valid" });
        }







        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {

            var user = await _userManager.FindByNameAsync(loginDto.UserName) ?? await _userManager.FindByEmailAsync(loginDto.UserName);


            if (user == null)
                return Conflict("Password or Email not found");

            if (await _userManager.IsLockedOutAsync(user))
                return StatusCode(403, $"User is locked out due to multiple failed login attempts. Please try again after {user.LockoutEnd?.LocalDateTime}");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);


            if (!user.EmailConfirmed)
                return BadRequest("Please confirm your email address.");


            if (!isPasswordValid)
            {
                await _userManager.AccessFailedAsync(user);


                if (await _userManager.IsLockedOutAsync(user))
                    return StatusCode(403, $"User is locked out due to multiple failed login attempts. Please try again after {user.LockoutEnd?.LocalDateTime}");

                return BadRequest("Invalid password or email.");
            }


            await _userManager.ResetAccessFailedCountAsync(user);


            if (user.LockoutEnd.HasValue && user.LockoutEnd.Value <= DateTime.UtcNow)
            {
                user.LockoutEnd = null;
                await _userManager.UpdateAsync(user);
            }


            var userRoles = await _userManager.GetRolesAsync(user);
            var token = _tokenService.GetToken(userRoles, user, _jwtSetting);

            return Ok(new { token });
        }



        [HttpPut("edit")]
        [Authorize]
        public async Task<IActionResult> EditUser([FromBody] EditUserDto editUserDto)
        {
            // JWT üzerinden kullanıcının ID'sini al
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Kullanıcı ID'sine göre kullanıcıyı bul
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            // Kullanıcı bilgilerini güncelle
            user.FullName = editUserDto.FullName;
            user.LastName = editUserDto.LastName;
            user.Img = editUserDto.Img ?? user.Img; // Var olan resmi koru

            // E-posta değişikliği kontrolü
            if (user.Email != editUserDto.Email)
            {
                var otpCode = GenerateOtp();
                user.ResetPasswordOtp = otpCode;
                user.OtpExpiryTime = DateTime.UtcNow.Add(otpExpiryDuration);
                await _userManager.UpdateAsync(user);

                // OTP'yi içeren bir e-posta gönder
                string body = $"Your OTP code for email change: {otpCode}";
                _emailService.SendEmail(new List<string> { user.Email }, "OTP Code for Email Change", body);

                return Ok(new { message = "OTP code sent to your email." });
            }

            await _userManager.UpdateAsync(user);
            return Ok(new { message = "User information updated successfully." });
        }
        [HttpPost("confirmEmailChange")]
        [Authorize]
        public async Task<IActionResult> ConfirmEmailChange(string email, string otpCode)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return NotFound();

            // OTP doğrulama
            if (user.ResetPasswordOtp != otpCode || user.OtpExpiryTime < DateTime.UtcNow)
            {
                return BadRequest("Invalid or expired OTP code.");
            }

            user.Email = user.Email; // Yeni e-posta adresi buraya yazılabilir
            user.ResetPasswordOtp = null; // OTP'yi sıfırla
            user.OtpExpiryTime = null;
            await _userManager.UpdateAsync(user);

            return Ok("Email changed successfully.");
        }

        private string GenerateOtp()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString(); // 6 haneli OTP üret
        }
        [HttpGet("google-login")]
        public IActionResult Login()
        {
            var redirectUrl = Url.Action("GoogleCallback", "Auth");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(GoogleDefaults.AuthenticationScheme, redirectUrl);
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }



        [HttpGet("google-callback")]
        public async Task<IActionResult> GoogleCallback()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return BadRequest("Google authentication failed.");
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var name = info.Principal.FindFirstValue(ClaimTypes.Name);
            var googleId = info.Principal.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new User
                {
                    UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                    Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                    FullName = info.Principal.FindFirstValue(ClaimTypes.Name),
                    LastName=name,
                    EmailConfirmed = true,
                    Img = "default.png",
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                };

                var randomPassword = GenerateRandomPassword();
                var createUserResult = await _userManager.CreateAsync(user, randomPassword);
                if (!createUserResult.Succeeded)
                {
                    var errors = createUserResult.Errors.Select(e => e.Description).ToList();
                    return BadRequest(errors);
                }


                var addLoginResult = await _userManager.AddLoginAsync(user, info);
                if (!addLoginResult.Succeeded)
                {
                    return BadRequest(addLoginResult.Errors);
                }

                await _userManager.AddToRoleAsync(user, "Student");
            }


            var userRoles = await _userManager.GetRolesAsync(user);


            // JWT Token Oluştur
            var token = await _tokenService.CreateTokenAsync(userRoles, user, _jwtSetting);

            // Sadece token'ı döndür
            return Ok(new { token });
        }

        private string GenerateRandomPassword(int length = 8)
        {
            const string lowerChars = "abcdefghijklmnopqrstuvwxyz";
            const string upperChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string digits = "1234567890";
            const string specialChars = "!@#$%^&*()";

            // En az bir karakter türünden almak için şifreyi başlat
            var random = new Random();
            var password = new List<char>
    {
        lowerChars[random.Next(lowerChars.Length)], // En az bir küçük harf
        upperChars[random.Next(upperChars.Length)], // En az bir büyük harf
        digits[random.Next(digits.Length)],         // En az bir rakam
        specialChars[random.Next(specialChars.Length)] // En az bir özel karakter
    };

            // Kalan karakterleri rastgele seç
            const string allChars = lowerChars + upperChars + digits + specialChars;
            for (int i = 4; i < length; i++)
            {
                password.Add(allChars[random.Next(allChars.Length)]);
            }

            // Şifreyi karıştır ve döndür
            return new string(password.OrderBy(x => random.Next()).ToArray());
        }

    }






}

