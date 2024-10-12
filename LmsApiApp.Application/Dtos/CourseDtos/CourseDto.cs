namespace LmsApiApp.Application.Dtos.CourseDtos
{
    public class CourseDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string? Img { get; set; }
        public bool IsDelete { get; set; }
        public DateTime? StartTime { get; set; }
        public List<string>? UserIds { get; set; }

        public int? GroupId { get; set; }

        public List<string>? CourseStudents { get; set; }  // List of student user IDs
        public List<string>? CourseTeachers { get; set; }
    }


}
