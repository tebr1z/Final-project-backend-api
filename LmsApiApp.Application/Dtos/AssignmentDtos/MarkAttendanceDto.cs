using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LmsApiApp.Application.Dtos.AssignmentDtos
{
    public class MarkAttendanceDto
    {
        public int CourseId { get; set; }
        public int StudentId { get; set; }
        public bool IsPresent { get; set; }
    }

}
