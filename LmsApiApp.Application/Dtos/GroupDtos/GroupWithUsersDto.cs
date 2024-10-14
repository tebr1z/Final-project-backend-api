using LmsApiApp.Application.Dtos.UserDtos;

namespace LmsApiApp.Application.Dtos.GroupDtos
{
    public class GroupWithUsersDto
    {
        public int Id { get; set; } // Grup ID'si
        public string Name { get; set; } // Grup ismi
        public string Description { get; set; } // Grup açıklaması
        public List<UserDto> Users { get; set; } // Gruplardaki kullanıcıların listesi
    }
}
