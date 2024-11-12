using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LmsApiApp.Application.Dtos.TestDtos
{
    public class TestResultDto
    {
        public int TestId { get; set; }
        public int Score { get; set; }
        public bool IsReshuffled { get; set; }
        public DateTime CompletedAt { get; set; }
        public bool IsApprovedByTeacher { get; set; }
        public List<QuestionResultDto> Questions { get; set; }
        public string UserId { get; set; }
    }
}
