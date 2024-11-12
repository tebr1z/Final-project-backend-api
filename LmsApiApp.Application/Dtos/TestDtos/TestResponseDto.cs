using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LmsApiApp.Application.Dtos.TestDtos
{
    public class TestResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string MediaUrl { get; set; }
        public TimeSpan TestDuration { get; set; }
        public bool IsTimed { get; set; }
        public ICollection<QuestionDto>? Questions { get; set; }
    }
}
