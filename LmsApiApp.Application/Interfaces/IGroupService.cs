using LmsApiApp.Application.Dtos.GroupDtos;
using LmsApiApp.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LmsApiApp.Application.Interfaces
{
    public interface IGroupService
    {
        Task<IEnumerable<Group>> GetAllGroupsAsync();
        Task<Group> GetGroupByIdAsync(int id);
        Task AddGroupAsync(Group group); // Yeni grup eklemek için
        Task UpdateGroupAsync(Group group); // Grup güncellemek için
        Task DeleteGroupAsync(int id); // Grup silmek için
        Task AddUserToGroupAsync(GroupEnrollment groupEnrollment);
        Task<IEnumerable<GroupWithUsersDto>> GetAllGroupsWithUsersAsync();
    }
}
