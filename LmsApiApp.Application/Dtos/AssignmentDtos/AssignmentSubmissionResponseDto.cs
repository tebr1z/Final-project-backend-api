using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LmsApiApp.Application.Dtos.AssignmentDtos
{
    public class AssignmentSubmissionResponseDto
    {
        public int AssignmentId { get; set; }
        public string SubmissionContent { get; set; }
        public string MediaUrl { get; set; }  
        public string UserId { get; set; }
        public double Grade { get; set; }

    }

}
