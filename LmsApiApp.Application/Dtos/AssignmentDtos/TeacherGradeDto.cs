using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LmsApiApp.Application.Dtos.AssignmentDtos
{
    public class TeacherGradeDto
    {
        public double Grade { get; set; }
        public string TeacherFeedback { get; set; }  // Öğretmen cevabı
    }


}
