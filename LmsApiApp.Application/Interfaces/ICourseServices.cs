using LmsApiApp.Application.Dtos.CourseDtos;
using LmsApiApp.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LmsApiApp.Application.Interfaces
{
    public interface ICourseServices
    {
        Task<List<CourseDto>> GetAllCoursesAsync(); 
        Task<CourseDto> GetCourseByIdAsync(int id); 
        Task AddCourseAsync(CourseDto courseDto);  
        Task UpdateCourseAsync(int id, CourseDto courseDto); 
        Task DeleteCourseAsync(int id);
       
        Task<List<CourseDto>> GetCoursesByUserIdAsync(string userId);

        Task<IEnumerable<CourseDto>> GetCoursesByUserAsync(string userId);
        Task<IEnumerable<CourseDto>> GetCoursesByGroupAsync(int groupId);
    }
}
