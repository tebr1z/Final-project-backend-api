using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LmsApiApp.Application.Dtos.UserDtos
{
    public class EditProfileDto
    {
        public string? FullName { get; set; }
        public string? LastName { get; set; }
        public string? Img { get; set; }
        public string? Role { get; set; }
        public string? Skills { get; set; }
        public string? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? InstagramLink { get; set; }
        public string? LinkedinLink { get; set; }
        public string? GithubLink { get; set; }
        public string? BehanceLink { get; set; }
    }

}
