namespace LmsApiApp.Application.Dtos.UserDtos
{
    public class UserDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? UserName { get; set; }

        public DateTime? LastActive { get; set; }  
        public bool? IsOnline { get; set; } 
    }
}
