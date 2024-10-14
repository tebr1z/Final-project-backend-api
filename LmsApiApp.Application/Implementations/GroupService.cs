using LmsApiApp.Application.Dtos.GroupDtos;
using LmsApiApp.Application.Dtos.UserDtos;
using LmsApiApp.Application.Interfaces;
using LmsApiApp.Core.Entities;
using LmsApiApp.DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace LmsApiApp.Application.Services
{
    public class GroupService : IGroupService
    {
        private readonly LmsApiDbContext _context;

        public GroupService(LmsApiDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Group>> GetAllGroupsAsync()
        {
            return await _context.Groups.ToListAsync(); // Tüm grupları getir
        }

        public async Task<Group> GetGroupByIdAsync(int id)
        {
            return await _context.Groups.FindAsync(id); // ID'ye göre grup bul
        }

        public async Task AddGroupAsync(Group group)
        {
            _context.Groups.Add(group); // Yeni grup ekle
            await _context.SaveChangesAsync(); // Veritabanına kaydet
        }

        public async Task UpdateGroupAsync(Group group)
        {
            _context.Groups.Update(group); // Grup güncelle
            await _context.SaveChangesAsync(); // Veritabanına kaydet
        }

        public async Task DeleteGroupAsync(int id)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group != null)
            {
                _context.Groups.Remove(group); // Grup sil
                await _context.SaveChangesAsync(); // Veritabanına kaydet
            }
        }
        // Kullanıcıyı gruba eklemek için metod
        public async Task AddUserToGroupAsync(GroupEnrollment groupEnrollment)
        {
            _context.GroupEnrollments.Add(groupEnrollment); // Gruba kullanıcı ekle
            await _context.SaveChangesAsync(); // Değişiklikleri veritabanına kaydet
        }

        public async Task<IEnumerable<GroupWithUsersDto>> GetAllGroupsWithUsersAsync()
        {
            var groups = await _context.Groups
                .Include(g => g.GroupEnrollments) // Grupla ilgili kullanıcı kayıtlarını dahil et
                .ThenInclude(ge => ge.User) // Her kullanıcı kaydından kullanıcı bilgisine git
                .ToListAsync();

            // DTO'ya dönüştürme işlemi
            return groups.Select(g => new GroupWithUsersDto
            {
                Id = g.Id,
                Name = g.Name,
               
                Users = g.GroupEnrollments.Select(ge => new UserDto
                {
                    Id = ge.User.Id,
                    Name = ge.User.UserName
                }).ToList()
            }).ToList();
        }
    }
}
