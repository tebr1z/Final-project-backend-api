using AutoMapper;
using LmsApiApp.Application.Dtos.CourseDtos;
using LmsApiApp.Application.Exceptions;
using LmsApiApp.Application.Interfaces;
using LmsApiApp.Core.Entities;
using LmsApiApp.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LmsApiApp.Application.Implementations
{
    public class CourseService : ICourseServices 
    {
        private readonly IMapper _mapper;
        private readonly ICourseRepository _courseRepository;
        private readonly LmsApiDbContext _context;
        public CourseService(IMapper mapper, ICourseRepository courseRepository, LmsApiDbContext context)
        {
            _mapper = mapper;
            _courseRepository = courseRepository;
            _context = context;
        }

        public async Task<List<CourseDto>> GetAllCoursesAsync()
        {
            var courses = await _courseRepository.GetAllAsync(); 
            return _mapper.Map<List<CourseDto>>(courses); 
        }
        public async Task<List<CourseDto>> GetCoursesByUserIdAsync(string userId)
        {
            var courses = await _context.Courses
                .Include(c => c.CourseTeachers)
                .Include(c => c.CourseStudents)
                .Where(c => c.CourseTeachers.Any(ct => ct.UserId == userId) || c.CourseStudents.Any(cs => cs.UserId == userId))
                .ToListAsync();

            return _mapper.Map<List<CourseDto>>(courses);
        }
        public async Task<CourseDto> GetCourseByIdAsync(int id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null) throw new CourseNotFoundException(id);
            return _mapper.Map<CourseDto>(course); 
        }


        public async Task AddCourseAsync(CourseDto courseDto)
        {
            // CourseDto -> Course entity dönüşümü
            var courseEntity = _mapper.Map<Course>(courseDto);

            // CreatedDate otomatik atanıyor
            courseEntity.CreatedDate = DateTime.UtcNow;

            await _courseRepository.AddAsync(courseEntity);
        }


        public async Task UpdateCourseAsync(int id, CourseDto courseDto)
        {
            var existingCourse = await _courseRepository.GetByIdAsync(id);
            if (existingCourse == null) throw new CourseNotFoundException(id);

            // Mevcut course'u güncelliyoruz, CourseDto -> Course dönüşümü
            _mapper.Map(courseDto, existingCourse);
            existingCourse.UpdateTime = DateTime.UtcNow; // UpdateTime otomatik ayarlanıyor

            await _courseRepository.UpdateAsync(existingCourse);
        }

        public async Task DeleteCourseAsync(int id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null) throw new CourseNotFoundException(id);

            await _courseRepository.DeleteAsync(course);
        }


        public async Task<IEnumerable<CourseDto>> GetCoursesByUserAsync(string userId)
        {
            return await _courseRepository.GetCoursesByUserAsync(userId);
        }

        public async Task<IEnumerable<CourseDto>> GetCoursesByGroupAsync(int groupId)
        {
            return await _courseRepository.GetCoursesByGroupAsync(groupId);
        }


    }



}
