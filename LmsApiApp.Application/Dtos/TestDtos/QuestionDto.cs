using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LmsApiApp.Application.Dtos.TestDtos
{
    public class QuestionDto
    {
        public string Text { get; set; }
        public string Type { get; set; }  // multiple-choice, open-ended
        public int Points { get; set; }
        public ICollection<AnswerDto> Answers { get; set; }
    }
}
