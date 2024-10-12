using System;

namespace LmsApiApp.Application.Dtos.AssignmentDtos
{
    public class AssignmentDto
    {

        public string Title { get; set; }
        public string Description { get; set; }
        public string? MediaUrl { get; set; } 
        public DateTime Deadline { get; set; } 
        public int CourseId { get; set; }
        public int? Grade { get; set; } 
    
        public bool IsResubmitted { get; set; }
        public string UserId { get; set; }
    }
}
