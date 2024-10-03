using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LmsApiApp.Core.Entities
{
    public class CourseStudent
    {
        public int CourseId { get; set; }
        public Course Course { get; set; }

        public string UserId { get; set; }  // IdentityUser'dan gelen Id (Student)
        public User Student { get; set; }
    }

}
