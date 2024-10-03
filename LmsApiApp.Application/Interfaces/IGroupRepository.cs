using LmsApiApp.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LmsApiApp.Application.Interfaces
{
    public interface IGroupRepository
    {
        Task<List<Group>> GetAllAsync();
        Task<Group> GetByIdAsync(int id); 
        Task AddAsync(Group group);    
        Task UpdateAsync(Group group);   
        Task DeleteAsync(Group group);   
    }
}
