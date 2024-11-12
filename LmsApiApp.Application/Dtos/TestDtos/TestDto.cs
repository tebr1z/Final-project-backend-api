using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LmsApiApp.Application.Dtos.TestDtos
{
    public class TestDto
    {
        public string Title { get; set; } // Test başlığı
        public string Content { get; set; } // Test içeriği
        public string MediaUrl { get; set; } // Medya URL'si
        public int CourseId { get; set; } // Kurs ID'si
        public string UserId { get; set; } // Kullanıcı ID'si, gerekliyse
        public TimeSpan TestDuration { get; set; } // Test süresi
        public bool IsTimed { get; set; } // Test zamanlı mı?
        public ICollection<QuestionDto> Questions { get; set; }
    }


}
