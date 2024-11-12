using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LmsApiApp.Application.Dtos.TestDtos
{
    public class QuestionResultDto
    {
        public int QuestionId { get; set; } // Sorunun ID'si
        public string GivenAnswer { get; set; } // Kullanıcının verdiği cevap
    }
}
