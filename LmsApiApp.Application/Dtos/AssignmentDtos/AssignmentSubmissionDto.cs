using System;

namespace LmsApiApp.Application.Dtos.AssignmentDtos
{
    public class AssignmentSubmissionDto
    {
        public int AssignmentId { get; set; } 
        public string SubmissionContent { get; set; } 

        
        public string? MediaUrl { get; set; } 

        public string UserId { get; set; }

    }
}
