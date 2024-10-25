﻿
using FluentValidation;
using System.ComponentModel.DataAnnotations;


namespace LmsApiApp.Application.Dtos.UserDtos
{

    public class RegisterDto
    {
        [Required]

        public string FullName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string RePassword { get; set; }

        public string Img { get; set; } = "default.png"; // Varsayılan resim
    }
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(r => r.FullName)
                    .NotEmpty()
                    .MaximumLength(30);
            RuleFor(r => r.LastName)
                   .NotEmpty()
                   .MaximumLength(30);

            RuleFor(r => r.UserName)
                .NotEmpty()
                .WithMessage("Username cannot be empty.")
                .Matches(@"^[a-zA-Z0-9]*$")
                .WithMessage("Username can only contain letters and digits.")
                .MaximumLength(30)
                .MinimumLength(3);


            RuleFor(r => r.Email)
                .NotEmpty()
                .EmailAddress();


            RuleFor(r => r.Password)
                .NotEmpty()
                .MinimumLength(6)
                .MaximumLength(30);


            RuleFor(r => r.RePassword)
               .NotEmpty()
               .MinimumLength(6)
               .MaximumLength(30);


            RuleFor(r => r.Password)
                .Custom((password, context) =>
                {
                    var rePassword = context.InstanceToValidate.RePassword;

                    if (password != rePassword)
                    {
                        context.AddFailure("Password", "Password and confirmation password do not match.");
                    }
                });
        }
    }

}
