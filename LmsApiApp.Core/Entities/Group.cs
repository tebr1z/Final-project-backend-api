using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LmsApiApp.Core.Entities
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<GroupEnrollment>? GroupEnrollments { get; set; } // Group members (students and teachers)
        public ICollection<Course>? Courses { get; set; } // Courses linked to the group
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
        public bool IsDelete { get; set; }

    }
}
