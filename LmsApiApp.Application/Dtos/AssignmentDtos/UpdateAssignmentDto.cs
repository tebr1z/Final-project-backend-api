using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LmsApiApp.Application.Dtos.AssignmentDtos
{
    public class UpdateAssignmentDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int CourseId { get; set; }
        public DateTime Deadline { get; set; }
        public string? MediaUrl { get; set; }  // Opsiyonel hale getirildi
        public string UserId { get; set; }
        public bool IsResubmitted { get; set; }
        public int? Grade { get; set; } // Öğretmen tarafından verilecek not

    }

}
