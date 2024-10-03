namespace LmsApiApp.Application.Dtos.GroupDtos
{
    public class GroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDelete { get; set; }
        public List<int> CourseIds { get; set; } // Grup ile ilişkili ders ID'leri
    }
}
