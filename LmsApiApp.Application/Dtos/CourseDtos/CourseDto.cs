namespace LmsApiApp.Application.Dtos.CourseDtos
{
    public class CourseDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Img { get; set; }
        public bool IsDelete { get; set; }
        public DateTime? StartTime { get; set; }
        public List<int>? GroupIds { get; set; }
        public List<string>? UserId { get; set; }
    }


}
