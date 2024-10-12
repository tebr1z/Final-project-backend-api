namespace LmsApiApp.Core.Entities
{
    public class Assignment
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string MediaUrl { get; set; }
      
        public int CourseId { get; set; }
        public Course Course { get; set; }

    
        public string UserId { get; set; }  

       
        public User User { get; set; }

        public DateTime Deadline { get; set; }

        
        public double? Grade { get; set; }

      
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

      
        public bool IsResubmitted { get; set; }
        public virtual ICollection<CourseStudent> CourseStudents { get; set; }  
        public virtual ICollection<CourseTeacher> CourseTeachers { get; set; }
        public virtual ICollection<AssignmentSubmission> Submissions { get; set; }
    }


}
