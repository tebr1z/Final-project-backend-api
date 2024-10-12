using LmsApiApp.Application.Dtos.CourseDtos;
using LmsApiApp.Application.Interfaces;
using LmsApiApp.Core.Entities;
using LmsApiApp.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LmsApiApp.Application.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly LmsApiDbContext _context;

        public CourseRepository(LmsApiDbContext context)
        {
            _context = context;
        }

        public async Task<List<Course>> GetAllAsync()
        {
            return await _context.Courses.ToListAsync();
        }

        public async Task<Course> GetByIdAsync(int id)
        {
            return await _context.Courses.FindAsync(id);
        }

        public async Task AddAsync(Course course)
        {
            await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Course course)
        {
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Course course)
        {
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
        }


        public async Task<IEnumerable<CourseDto>> GetCoursesByUserAsync(string userId)
        {
            return await _context.Courses
                .Where(c => c.UserId == userId)
                .Select(c => new CourseDto
                {
                 
                    Title = c.Title,
                    Description = c.Description,
                    Img = c.Img
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<CourseDto>> GetCoursesByGroupAsync(int groupId) // Eksik metot burada implement ediliyor
        {
            return await _context.Courses
                .Where(c => c.GroupId == groupId)
                .Select(c => new CourseDto
                {
                    Title = c.Title,
                    Description = c.Description,
                    Img = c.Img
                })
                .ToListAsync();
        }
    }
}
