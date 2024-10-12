using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LmsApiApp.Application.Dtos.CourseDtos
{
    public class CourseSoftDeleteDto
    {
        public int Id { get; set; }
        public bool IsDelete { get; set; } = true;  
    }
}
