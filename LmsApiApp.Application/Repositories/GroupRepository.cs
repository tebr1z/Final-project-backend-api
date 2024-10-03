using LmsApiApp.Application.Interfaces;
using LmsApiApp.Core.Entities;
using LmsApiApp.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LmsApiApp.Application.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private readonly LmsApiDbContext _context;

        public GroupRepository(LmsApiDbContext context)
        {
            _context = context;
        }

        public async Task<List<Group>> GetAllAsync()
        {
            return await _context.Groups.ToListAsync();
        }

        public async Task<Group> GetByIdAsync(int id)
        {
            return await _context.Groups.FindAsync(id);
        }

        public async Task AddAsync(Group group)
        {
            await _context.Groups.AddAsync(group); 
            await _context.SaveChangesAsync(); 
        }

        public async Task UpdateAsync(Group group)
        {
            _context.Groups.Update(group); 
            await _context.SaveChangesAsync(); 
        }

        public async Task DeleteAsync(Group group)
        {
            _context.Groups.Remove(group); 
            await _context.SaveChangesAsync(); 
        }
    }
}
