﻿using LmsApiApp.Application.Dtos.CourseDtos;
using LmsApiApp.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LmsApiApp.Application.Interfaces
{
    public interface ICourseRepository
    {
        Task<List<Course>> GetAllAsync();
        Task<Course> GetByIdAsync(int id); 
        Task AddAsync(Course course);      
        Task UpdateAsync(Course course);  
        Task DeleteAsync(Course course);
        Task<IEnumerable<CourseDto>> GetCoursesByUserAsync(string userId);
        Task<IEnumerable<CourseDto>> GetCoursesByGroupAsync(int groupId);
    }
}
