using System.ComponentModel.DataAnnotations;

public class ResetPasswordDto
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string OtpCode { get; set; }

    [Required]
    [MinLength(8, ErrorMessage = "Şifre en az 8 karakter olmalıdır.")]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@.#$]).{8,}$",
        ErrorMessage = "Şifre en az bir büyük harf, bir küçük harf, bir sayı ve bir özel karakter (!@.#$) içermelidir.")]
    public string Password { get; set; }

    [Required]
    [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor!")]
    public string RePassword { get; set; }
}
