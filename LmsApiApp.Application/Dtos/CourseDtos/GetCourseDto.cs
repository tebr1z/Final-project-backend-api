using LmsApiApp.Application.Dtos.CourseDtos;

public class GetCourseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Img { get; set; }
    public ICollection<CourseStudentDto> CourseStudents { get; set; }
    public ICollection<CourseTeacherDto> CourseTeachers { get; set; }
}
