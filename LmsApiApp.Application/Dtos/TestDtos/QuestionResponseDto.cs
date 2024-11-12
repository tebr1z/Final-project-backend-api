using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LmsApiApp.Application.Dtos.TestDtos
{
    public class QuestionResponseDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Type { get; set; }
        public int Points { get; set; }
        public List<AnswerResponseDto> Answers { get; set; }
    }
}
