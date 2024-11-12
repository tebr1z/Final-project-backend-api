using LmsApiApp.Application.Dtos.UserDtos;
using LmsApiApp.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LmsApiApp.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetUsersAsync(string search); // Kullanıcıları getir
        Task<User> GetUserByIdAsync(string userId);
        Task SetUserOnlineStatusAsync(string userId, bool isOnline);
        Task UpdateLastActiveTimeAsync(string userId);
        Task SendOfflineNotificationAsync(User user, Message message);
        Task<DateTime?> GetLastActiveTimeAsync(string userId); // Yeni metod
    }
}
