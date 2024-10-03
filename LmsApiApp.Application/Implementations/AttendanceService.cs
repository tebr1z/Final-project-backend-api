using LmsApiApp.Application.Interfaces;
using LmsApiApp.Core.Entities;
using LmsApiApp.DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LmsApiApp.Application.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly LmsApiDbContext _context;
        private readonly UserManager<User> _userManager;

        public AttendanceService(LmsApiDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Kullanıcının sahip olduğu kurslardaki yoklamaları getir (CourseId'ye göre)
        public async Task<IEnumerable<Attendance>> GetAllAttendancesAsync(string userId)
        {
            // Kullanıcının rolü yerine, onun sahip olduğu kurslardaki yoklamaları döndürüyoruz
            var userCourses = await _context.Courses
                .Include(c => c.CourseTeachers)
                .Where(c => c.CourseTeachers.Any(ct => ct.UserId == userId))  // Kurs sahipliği kontrolü
                .Select(c => c.Id)
                .ToListAsync();

            // Kullanıcının sahip olduğu kurslara göre yoklamaları döndürüyoruz
            return await _context.Attendances
                .Include(a => a.Course)
                .Where(a => userCourses.Contains(a.CourseId))  // Kullanıcının kurslarına ait yoklamalar
                .ToListAsync();
        }

        // Belirli bir yoklamayı getir, sadece kullanıcıya ait kurs yoklamalarını döndür
        public async Task<Attendance> GetAttendanceByIdAsync(int id, string userId)
        {
            // Kullanıcının sahip olduğu kursları alıyoruz
            var userCourses = await _context.Courses
                .Include(c => c.CourseTeachers)
                .Where(c => c.CourseTeachers.Any(ct => ct.UserId == userId))  // Kurs sahipliği kontrolü
                .Select(c => c.Id)
                .ToListAsync();

            // Yoklamayı getiriyoruz
            return await _context.Attendances
                .Include(a => a.Course)
                .Where(a => a.Id == id && userCourses.Contains(a.CourseId))  // Kullanıcıya ait kurs yoklamalarını döndür
                .FirstOrDefaultAsync();
        }

        // Yeni yoklama oluştur (CourseId yeterli)
        public async Task AddAttendanceAsync(int courseId)
        {
            var attendance = new Attendance
            {
                CourseId = courseId,
                Date = DateTime.Now,
                IsPresent = true  // Default olarak 'Present' olarak işaretlenebilir
            };

            _context.Attendances.Add(attendance);
            await _context.SaveChangesAsync();
        }

        // Mevcut yoklamayı güncelle
        public async Task UpdateAttendanceAsync(int id, Attendance attendance)
        {
            var existingAttendance = await _context.Attendances.FindAsync(id);
            if (existingAttendance != null)
            {
                existingAttendance.IsPresent = attendance.IsPresent;
                existingAttendance.Date = attendance.Date;
                existingAttendance.ExcuseDocument = attendance.ExcuseDocument;

                await _context.SaveChangesAsync();
            }
        }

        // Yoklamayı sil
        public async Task DeleteAttendanceAsync(int id)
        {
            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance != null)
            {
                _context.Attendances.Remove(attendance);
                await _context.SaveChangesAsync();
            }
        }
    }
}
