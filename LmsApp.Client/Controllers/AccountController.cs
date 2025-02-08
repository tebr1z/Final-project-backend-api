using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;
using System.Text;
using LmsApp.Client.Models;
using Microsoft.AspNetCore.Http;

namespace LmsApp.Client.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("LmsApi");
        }

        // Kullanıcı kayıt sayfası
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // JSON Verisini Konsola Yazdır (Debug için)
            var json = JsonSerializer.Serialize(model);
            Console.WriteLine("Gönderilen JSON: " + json);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("api/auth/register", content);
                var responseContent = await response.Content.ReadAsStringAsync(); // API'nin cevabını al

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Login");
                }

                // Hata mesajını ekrana göster
                ModelState.AddModelError("", "Kayıt başarısız! API Hatası: " + responseContent);
                Console.WriteLine("Backend Hatası: " + responseContent); // Hata mesajını konsola yazdır

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Sunucuya bağlanırken hata oluştu: " + ex.Message);
                Console.WriteLine("Exception: " + ex.Message);
            }

            return View(model);
        }




        // Giriş Sayfası
        public IActionResult Login()
        {
            return View();
        }

        // Kullanıcı giriş işlemi (API'ye istek atar)
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/auth/login", content);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı!");
                return View(model);
            }

            var responseData = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.UserName),
                new Claim("Token", tokenResponse.Token)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties { IsPersistent = true };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            return RedirectToAction("Index", "Home");
        }

        // Kullanıcı çıkış işlemi
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }


        // Email doğrulama linki açıldığında çalışacak metot
        [HttpGet]
        public async Task<IActionResult> VerifyEmail(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                ViewBag.Success = false;
                return View();
            }

            string apiUrl = $"https://localhost:7162/api/Auth/verifyEmail?email={Uri.EscapeDataString(email)}&token={Uri.EscapeDataString(token)}";
            var response = await _httpClient.GetAsync(apiUrl);

            ViewBag.Success = response.IsSuccessStatusCode;

            return View();
        }


        // Password Reset//
        // 1️⃣ **Şifre sıfırlamak için e-posta OTP gönderme**
        [HttpPost]
        public async Task<IActionResult> SendOtp(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ViewBag.Error = "Email adresi gereklidir!";
                return View("ForgotPassword");
            }

            var response = await _httpClient.PostAsync($"api/Auth/resetPasswordSendOtp?email={email}", null);
            if (response.IsSuccessStatusCode)
            {
                TempData["Email"] = email;
                return RedirectToAction("VerifyOtp");
            }

            ViewBag.Error = "Bu e-posta adresi kayıtlı değil.";
            return View("ForgotPassword");
        }

        [HttpGet]
        public IActionResult VerifyOtp()
        {
            ViewBag.Email = TempData["Email"] as string;
            return View();
        }

        [HttpPost("VerifyOtp")]
        public async Task<IActionResult> VerifyOtp([FromBody] OTPRequest model)
        {
            string email = TempData["Email"] as string;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(model.OtpCode))
            {
                return BadRequest(new { message = "Email veya OTP kodu eksik!" });
            }

            var response = await _httpClient.PostAsync($"api/Auth/verifyOtp?email={email}&otpCode={model.OtpCode}", null);
            string responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                TempData["Email"] = email;
                return Ok(new { message = "OTP doğrulandı, yönlendiriliyorsunuz..." });
            }

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return BadRequest(new { message = "OTP hatalı veya süresi geçmiş!" });
            }

            return StatusCode((int)response.StatusCode, new { message = "Sunucu hatası, lütfen tekrar deneyin." });
        }
        public class OTPRequest
        {
            public string OtpCode { get; set; }
        }
        [HttpGet]
        public IActionResult ResetPassword()
        {
            ViewBag.Email = TempData["Email"] as string;
            ViewBag.OtpCode = TempData["OtpCode"] as string;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"api/Auth/resetPasswordWithOtp?email={model.Email}&otpCode={model.OtpCode}", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Login");
            }

            ViewBag.Error = "Şifre sıfırlama başarısız oldu.";
            return View();
        }

        /// **Şifremi unuttum ekranını aç**
        public IActionResult ForgotPassword()
        {
            return View();
        }


    }
}
