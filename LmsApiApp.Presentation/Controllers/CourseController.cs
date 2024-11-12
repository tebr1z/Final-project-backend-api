using AutoMapper;
using LmsApiApp.Application.Dtos.CourseDtos;
using LmsApiApp.Application.Interfaces;
using LmsApiApp.Application.Services;
using LmsApiApp.Core.Entities;
using LmsApiApp.DataAccess.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class CourseController : ControllerBase
{
    private readonly ICourseServices _courseService;
    private readonly IFileUploadService _fileUploadService;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private readonly LmsApiDbContext _context;

    public CourseController(
        ICourseServices courseService,
        IFileUploadService fileUploadService,
        UserManager<User> userManager,
        IMapper mapper,
        LmsApiDbContext context)
    {
        _courseService = courseService;
        _fileUploadService = fileUploadService;
        _userManager = userManager;
        _mapper = mapper;
        _context = context;
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<CourseDto>> GetCourse(int id)
    {
        var courseEntity = await _courseService.GetCourseByIdAsync(id);

        if (courseEntity == null)
        {
            return NotFound();
        }

       
        var courseDto = _mapper.Map<CourseDto>(courseEntity);
        return Ok(courseDto);
    }
    [HttpPost]
    public async Task<ActionResult> CreateCourse([FromForm] CourseDto courseDto, IFormFile mediaFile)
    {
        if (courseDto.GroupId == null && (courseDto.UserIds == null || !courseDto.UserIds.Any()))
        {
            return BadRequest("Either GroupId or at least one UserId is required.");
        }

        if (mediaFile != null)
        {
            var mediaUrl = await _fileUploadService.UploadFileAsync(mediaFile);
            courseDto.Img = mediaUrl;
        }

        try
        {
         
            var courseEntity = _mapper.Map<Course>(courseDto);
            _context.Courses.Add(courseEntity);
            await _context.SaveChangesAsync(); 

         
            if (courseDto.GroupId.HasValue)
            {
                var groupEnrollments = await _context.GroupEnrollments
                    .Include(e => e.User) 
                    .Where(e => e.GroupId == courseDto.GroupId.Value)
                    .ToListAsync();

                if (groupEnrollments == null || !groupEnrollments.Any())
                {
                    return NotFound("No enrollments found for this group.");
                }

                foreach (var enrollment in groupEnrollments)
                {
                    var user = enrollment.User;
                    if (user == null)
                    {
                        Console.WriteLine("User is null, skipping...");
                        continue; 
                    }

                   
                    if (courseEntity.CourseStudents == null)
                    {
                        courseEntity.CourseStudents = new List<CourseStudent>();
                    }

                 
                    courseEntity.CourseStudents.Add(new CourseStudent
                    {
                        CourseId = courseEntity.Id,
                        UserId = user.Id
                    });
                }

               
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction(nameof(GetCourse), new { id = courseEntity.Id }, courseDto);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message, innerException = ex.InnerException?.Message });
        }
    }


    [HttpPost("assign-group-users-to-course/{courseId}/{groupId}")]
    public async Task<IActionResult> AssignGroupUsersToCourse(int courseId, int groupId)
    {
        var course = await _context.Courses
                                   .Include(c => c.CourseStudents)
                                   .Include(c => c.CourseTeachers)
                                   .FirstOrDefaultAsync(c => c.Id == courseId);
        if (course == null) return NotFound("Course not found.");

        var group = await _context.Groups
                                  .Include(g => g.GroupEnrollments)
                                  .ThenInclude(e => e.User)
                                  .FirstOrDefaultAsync(g => g.Id == groupId);

        if (group == null) return NotFound("Group not found.");

        foreach (var enrollment in group.GroupEnrollments)
        {
            var user = enrollment.User;
            var roles = await _userManager.GetRolesAsync(user);

            // Eğer kullanıcı rolü "Teacher" ise CourseTeacher'a ekle
            if (roles.Contains("Teacher"))
            {
                if (!course.CourseTeachers.Any(ct => ct.UserId == user.Id))
                {
                    course.CourseTeachers.Add(new CourseTeacher
                    {
                        CourseId = courseId,
                        UserId = user.Id,
                    });
                }
            }
            // Eğer kullanıcı rolü "Student" ise CourseStudent'a ekle
            else if (roles.Contains("Student"))
            {
                if (!course.CourseStudents.Any(cs => cs.UserId == user.Id))
                {
                    course.CourseStudents.Add(new CourseStudent
                    {
                        CourseId = courseId,
                        UserId = user.Id,
                    });
                }
            }
        }

        await _context.SaveChangesAsync();
        return Ok(course);
    }




    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCourse(int id, [FromForm] CourseDto courseDto, IFormFile mediaFile)
    {
        if (courseDto.GroupId == null && (courseDto.UserIds == null || !courseDto.UserIds.Any()))
        {
            return BadRequest("Either GroupId or at least one UserId is required.");
        }

        var existingCourse = await _courseService.GetCourseByIdAsync(id);
        if (existingCourse == null) return NotFound();

        if (mediaFile != null)
        {
            if (!string.IsNullOrEmpty(existingCourse.Img))
            {
                await _fileUploadService.DeleteFileAsync(existingCourse.Img);
            }

            var mediaUrl = await _fileUploadService.UploadFileAsync(mediaFile);
            courseDto.Img = mediaUrl;
        }
        else
        {
            courseDto.Img = existingCourse.Img;
        }

        try
        {
            await _courseService.UpdateCourseAsync(id, courseDto);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message, innerException = ex.InnerException?.Message });
        }
    }



    // Kursa öğrenci ekle
    [HttpPost("{courseId}/add-user")]
    public async Task<IActionResult> AddUserToCourse(int courseId, [FromBody] string userId)
    {
        // Kullanıcının var olup olmadığını kontrol et
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound("Kullanıcı bulunamadı.");
        }

        // Kursu kontrol et
        var course = await _context.Courses.Include(c => c.CourseStudents).FirstOrDefaultAsync(c => c.Id == courseId);
        if (course == null)
        {
            return NotFound("Kurs bulunamadı.");
        }

        // Kullanıcı zaten kursa eklenmiş mi kontrol et
        var isUserInCourse = course.CourseStudents.Any(cs => cs.UserId == userId);
        if (isUserInCourse)
        {
            return BadRequest("Kullanıcı zaten bu kursa eklenmiş.");
        }

        // Yeni CourseStudent oluştur ve ekle
        var courseStudent = new CourseStudent
        {
            CourseId = course.Id,
            UserId = userId
        };
        _context.CourseStudents.Add(courseStudent);

        // Veritabanına değişiklikleri kaydet
        await _context.SaveChangesAsync();

        return Ok(courseStudent);
    }

    [HttpPost("{courseId}/add-teacher")]
    public async Task<IActionResult> AddTeacherToCourse(int courseId, [FromBody] string teacherId)
    {
        // Öğretmenin var olup olmadığını kontrol et
        var teacher = await _userManager.FindByIdAsync(teacherId);
        if (teacher == null)
        {
            return NotFound("Öğretmen bulunamadı.");
        }

        // Kursu kontrol et
        var course = await _context.Courses.Include(c => c.CourseTeachers).FirstOrDefaultAsync(c => c.Id == courseId);
        if (course == null)
        {
            return NotFound("Kurs bulunamadı.");
        }

        // Öğretmen zaten kursa eklenmiş mi kontrol et
        var isTeacherInCourse = course.CourseTeachers.Any(ct => ct.UserId == teacherId);
        if (isTeacherInCourse)
        {
            return BadRequest("Öğretmen zaten bu kursa eklenmiş.");
        }

        // Yeni CourseTeacher oluştur ve ekle
        var courseTeacher = new CourseTeacher
        {
            CourseId = course.Id,
            UserId = teacherId
        };
        _context.CourseTeachers.Add(courseTeacher);

        // Veritabanına değişiklikleri kaydet
        await _context.SaveChangesAsync();

        return Ok(courseTeacher);
    }

    [HttpGet("teacher-courses/{teacherId}")]
    public async Task<IActionResult> GetTeacherCourses(string teacherId)
    {
        // Öğretmenin var olup olmadığını kontrol ediyoruz
        var teacher = await _userManager.FindByIdAsync(teacherId);
        if (teacher == null)
        {
            return NotFound("Öğretmen bulunamadı.");
        }

        // Öğretmenin kurslarını alıyoruz
        var courses = await _context.Courses
                                    .Include(c => c.CourseTeachers)
                                    .Where(c => c.CourseTeachers.Any(ct => ct.UserId == teacherId))
                                    .ToListAsync();

        // Kursları döndürüyoruz
        return Ok(courses);
    }




    [HttpGet("GetCoursesByUser/{userId}")]
    public async Task<IActionResult> GetCoursesByUser(string userId)
    {
        var courses = await _courseService.GetCoursesByUserAsync(userId);
        if (courses == null || !courses.Any())
        {
            return NotFound("Kullanıcıya ait kurs bulunamadı.");
        }
        return Ok(courses);
    }



    [HttpGet("GetAllCoursesWithUsers")]
    public async Task<IActionResult> GetAllCoursesWithUsers()
    {
        // Tüm kursları ve ilişkili öğrencileri ve öğretmenleri alıyoruz
        var courses = await _context.Courses
            .Include(c => c.CourseStudents)
                .ThenInclude(cs => cs.Student) // Öğrenci bilgilerini dahil et
            .Include(c => c.CourseTeachers)
                .ThenInclude(ct => ct.Teacher) // Öğretmen bilgilerini dahil et
            .ToListAsync();

        // DTO'ya dönüştürme
        var courseDtos = courses.Select(course => new GetCourseDto
        {
            Id = course.Id,
            Name = course.Description,
            Img = course.Img,
            CourseStudents = course.CourseStudents.Select(cs => new CourseStudentDto
            {
                UserId = cs.UserId,
                UserName = cs.Student.UserName // Kullanıcının adı
            }).ToList(),
            CourseTeachers = course.CourseTeachers.Select(ct => new CourseTeacherDto
            {
                UserId = ct.UserId,
                UserName = ct.Teacher.UserName // Kullanıcının adı
            }).ToList()
        }).ToList();

        return Ok(courseDtos);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCourse(int id)
    {
        // Kursu veritabanından bul
        var courseEntity = await _context.Courses
            .Include(c => c.CourseStudents)
            .Include(c => c.CourseTeachers)
            .FirstOrDefaultAsync(c => c.Id == id);

        // Eğer kurs bulunamazsa 404 döndür
        if (courseEntity == null)
        {
            return NotFound("Course not found.");
        }

        // Fotoğraf dosyasını sil
        if (!string.IsNullOrEmpty(courseEntity.Img))
        {
            // FileUploadService nesnesini oluştur
            var fileUploadService = new FileUploadService();
            await fileUploadService.DeleteFileAsync(courseEntity.Img); // Fotoğrafı sil
        }

        // Tüm CourseStudent kayıtlarını sil
        _context.CourseStudents.RemoveRange(courseEntity.CourseStudents);

        // Tüm CourseTeacher kayıtlarını sil
        _context.CourseTeachers.RemoveRange(courseEntity.CourseTeachers);

        // Kursu sil
        _context.Courses.Remove(courseEntity);

        // Değişiklikleri veritabanına kaydet
        await _context.SaveChangesAsync();

        // Başarılı silme işlemi için 204 No Content döndür
        return NoContent();
    }


}



