using LmsApiApp.Application.Dtos.UserDtos;
using LmsApiApp.Application.Interfaces;
using LmsApiApp.DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace LmsApiApp.Application.Services
{
    public class UserService : IUserService
    {
        private readonly LmsApiDbContext _context;

        public UserService(LmsApiDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserDto>> GetUsersAsync(string search)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(u => u.UserName.Contains(search)); // Arama işlemi
            }

            var users = await query.ToListAsync();

            // UserDto'ya dönüştürme işlemi
            return users.Select(u => new UserDto
            {
                Id = u.Id,
                Name = u.UserName
            }).ToList();
        }
    }
}
