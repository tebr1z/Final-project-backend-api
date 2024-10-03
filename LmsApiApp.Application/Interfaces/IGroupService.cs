using LmsApiApp.Application.Dtos.GroupDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LmsApiApp.Application.Interfaces
{
    public interface IGroupService
    {
        Task<List<GroupDto>> GetAllGroupsAsync(); 
        Task<GroupDto> GetGroupByIdAsync(int id); 
        Task AddGroupAsync(GroupDto groupDto);   
        Task UpdateGroupAsync(int id, GroupDto groupDto); 
        Task DeleteGroupAsync(int id); 
    }
}
