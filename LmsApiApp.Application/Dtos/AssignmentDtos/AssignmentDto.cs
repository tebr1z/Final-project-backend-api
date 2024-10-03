using System;

namespace LmsApiApp.Application.Dtos.AssignmentDtos
{
    public class AssignmentDto
    {

        public string Title { get; set; }
        public string Description { get; set; }
        public string? MediaUrl { get; set; } // Tek bir medya URL'si
        public DateTime Deadline { get; set; } // Son teslim tarihi
        public int CourseId { get; set; }
        public int? Grade { get; set; } // Öğretmen tarafından verilecek not
    
        public bool IsResubmitted { get; set; }
        public string UserId { get; set; }
    }
}
