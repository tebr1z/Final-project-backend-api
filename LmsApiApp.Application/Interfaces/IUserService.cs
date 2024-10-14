using LmsApiApp.Application.Dtos.UserDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LmsApiApp.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetUsersAsync(string search); // Kullanıcıları getir
    }
}
