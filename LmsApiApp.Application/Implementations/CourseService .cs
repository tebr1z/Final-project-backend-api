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

        public async Task<CourseDto> GetCourseByIdAsync(int id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null) throw new CourseNotFoundException(id);
            return _mapper.Map<CourseDto>(course); 
        }


        public async Task AddCourseAsync(CourseDto courseDto)
        {
            var course = _mapper.Map<Course>(courseDto); // DTO'dan Entity'ye dönüşüm
            course.CreatedDate = DateTime.UtcNow; // Otomatik olarak oluşturulma tarihi ekliyoruz
            await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateCourseAsync(int id, CourseDto courseDto)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null) throw new CourseNotFoundException(id);

            _mapper.Map(courseDto, course); 
            await _courseRepository.UpdateAsync(course);
        }

        public async Task DeleteCourseAsync(int id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null) throw new CourseNotFoundException(id);

            await _courseRepository.DeleteAsync(course);
        }
    }
}
