using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LmsApiApp.Application.Dtos.UserDtos
{
    public class ResetPasswordDto
    {
        public string Password { get; set; }
        public string RePassword { get; set; }
    }
}
