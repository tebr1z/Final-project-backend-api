using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace LmsApiApp.Core.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Img { get; set; }
        public ICollection<Assignment> Assignments { get; set; }
        public ICollection<Test> Tests { get; set; }

        public int? GroupId { get; set; }
        public Group Group { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public virtual ICollection<CourseStudent> CourseStudents { get; set; }

        public  DateTime CreatedDate { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime? StartTime { get; set; }
        public virtual ICollection<CourseTeacher> CourseTeachers { get; set; }

        public bool IsDelete { get; set; }

    }

}
