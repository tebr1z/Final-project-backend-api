namespace LmsApiApp.Core.Entities
{
   public class Test
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string  MediaUrl{ get; set; }
        public int CourseId { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public Course Course { get; set; }
        public virtual ICollection<TestResult> TestResults { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDelete { get; set; }

    }


}
