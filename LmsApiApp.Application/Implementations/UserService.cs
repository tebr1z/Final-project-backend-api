using LmsApiApp.Application.Dtos.UserDtos;
using LmsApiApp.Application.Implementations;
using LmsApiApp.Application.Interfaces;
using LmsApiApp.Core.Entities;
using LmsApiApp.DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LmsApiApp.Application.Services
{
    public class UserService : IUserService
    {
        private readonly LmsApiDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;
        public UserService(LmsApiDbContext context, UserManager<User> userManager, IEmailService emailService)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task<IEnumerable<UserDto>> GetUsersAsync(string search)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(u => u.UserName.Contains(search)); // Arama işlemi
            }

            // Sadece ihtiyaç duyduğunuz alanları veritabanından çekmek performans açısından daha iyidir
            var users = await query
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Name = u.UserName
                    // Gerekli diğer alanları ekleyin
                })
                .ToListAsync();

            return users;
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                // Hata veya loglama mekanizması ekleyin
                throw new Exception("User not found");
            }
            return user;
        }


        public async Task SetUserOnlineStatusAsync(string userId, bool isOnline)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.IsOnline = isOnline; // Kullanıcının online durumu
                user.LastActive = DateTime.UtcNow; // Son aktif zaman

                var result = await _userManager.UpdateAsync(user); // SQL'e güncelleme

                if (!result.Succeeded)
                {
                    // Güncelleme hatalarını loglayın
                    var errors = result.Errors.Select(e => e.Description).ToList();
                    Console.WriteLine($"Kullanıcı güncellenemedi: {string.Join(", ", errors)}");
                }
                else
                {
                    Console.WriteLine($"Kullanıcı {userId} online olarak güncellendi.");
                }
            }
            else
            {
                Console.WriteLine($"Kullanıcı {userId} bulunamadı.");
            }
        }




        public async Task SendOfflineNotificationAsync(User user, Message message)
        {
            if (!user.IsOnline)
            {
                var subject = "Yeni Mesaj Bildirimi";
                var body = $"Merhaba {user.FullName}, yeni bir mesaj aldınız: {message.Content}";
                // E-posta adresini List<string> olarak gönderiyoruz
                 _emailService.SendEmail(new List<string> { user.Email }, subject, body);
            }
        }




        // Bu kısım düzeltildi
        public async Task AddUserToMultipleRolesAsync(User user)
        {
            // Bir string yerine List<string> kullanıyoruz
            var roles = new List<string> { "Admin", "User", "Manager" };
            await _userManager.AddToRolesAsync(user, roles);  // Birden fazla rol ekleme
        }



        // Kullanıcının son aktif olduğu zamanı güncelle
        public async Task UpdateLastActiveTimeAsync(string userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user != null)
            {
                user.LastActive = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<UserDto>> GetOnlineUsersAsync()
        {
            return await _context.Users
                                 .Select(u => new UserDto
                                 {
                                     Id = u.Id,
                                     Name = u.UserName,
                                     IsOnline = u.IsOnline,
                                     LastActive = u.LastActive
                                 })
                                 .ToListAsync();
        }
        public async Task<DateTime?> GetLastActiveTimeAsync(string userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            return user?.LastActive; // Kullanıcının LastActive alanını döndür
        }

    }
}
