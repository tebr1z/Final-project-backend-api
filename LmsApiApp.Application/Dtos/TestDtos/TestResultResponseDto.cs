using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LmsApiApp.Application.Dtos.TestDtos
{
    public class TestResultResponseDto
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public int Score { get; set; }
        public bool IsReshuffled { get; set; }
        public DateTime CompletedAt { get; set; }
        public bool IsApprovedByTeacher { get; set; }
    }
}
