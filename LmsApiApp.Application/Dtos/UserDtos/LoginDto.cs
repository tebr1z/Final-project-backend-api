using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LmsApiApp.Application.Dtos.UserDtos
{
    public class LoginDto
    {

        public string UserName { get; set; }


        public string Password { get; set; }

    }
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(r => r.UserName)
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(50);


            RuleFor(r => r.Password)
                .NotEmpty()
                .MinimumLength(6)
                .MaximumLength(30);


        }
    }
}
