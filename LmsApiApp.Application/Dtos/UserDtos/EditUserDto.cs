﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LmsApiApp.Application.Dtos.UserDtos
{
    public class EditUserDto
    {
        public string Id { get; set; }
        public string? FullName { get; set; }
        public string ?LastName { get; set; }
        public string ?UserName { get; set; }
        public string? Email { get; set; }
        public string ?Password { get; set; }
        public string? RePassword { get; set; }

        public string? Img { get; set; } 
    }
  
}
