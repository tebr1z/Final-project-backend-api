namespace LmsApiApp.Core.Entities
{
    public class CourseTeacher
    {
        public int CourseId { get; set; }
        public Course Course { get; set; }

        public string UserId { get; set; }  // IdentityUser'dan gelen Id (Teacher)
        public User Teacher { get; set; }
    }

}
