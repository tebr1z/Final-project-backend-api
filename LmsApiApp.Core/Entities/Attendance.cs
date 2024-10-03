namespace LmsApiApp.Core.Entities
{
    public class Attendance
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public int StudentId { get; set; }
        public User Student { get; set; }
        public DateTime Date { get; set; }
        public bool IsPresent { get; set; }
        public string ExcuseDocument { get; set; }  // Path to the document for excused absences
    }

}
