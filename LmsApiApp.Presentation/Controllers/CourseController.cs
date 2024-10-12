using AutoMapper;
using LmsApiApp.Application.Dtos.CourseDtos;
using LmsApiApp.Application.Interfaces;
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

        // Entity'den DTO'ya dönüşüm yapıyoruz
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

        // Eğer bir medya dosyası yüklendiyse
        if (mediaFile != null)
        {
            var mediaUrl = await _fileUploadService.UploadFileAsync(mediaFile);
            courseDto.Img = mediaUrl;
        }

        try
        {
            // Kursu ekliyoruz
            await _courseService.AddCourseAsync(courseDto);
            return CreatedAtAction(nameof(GetCourse), new { id = courseDto }, courseDto);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message, innerException = ex.InnerException?.Message });
        }
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

    [HttpPost("assign-group-users-to-course/{courseId}/{groupId}")]
    public async Task<IActionResult> AssignGroupUsersToCourse(int courseId, int groupId)
    {
        var course = await _context.Courses.FindAsync(courseId);
        if (course == null) return NotFound("Course not found.");

        var group = await _context.Groups
            .Include(g => g.GroupEnrollments)
            .ThenInclude(e => e.User)
            .FirstOrDefaultAsync(g => g.Id == groupId);

        if (group == null) return NotFound("Group not found.");

        foreach (var enrollment in group.GroupEnrollments)
        {
            // Kullanıcıyı course students listesine ekle
            if (!course.CourseStudents.Any(cs => cs.UserId == enrollment.UserId))
            {
                course.CourseStudents.Add(new CourseStudent
                {
                    UserId = enrollment.UserId,
                    CourseId = courseId
                });
            }
        }

        await _context.SaveChangesAsync();
        return Ok(course);
    }

    [HttpGet("user-courses/{userId}")]
    public async Task<IActionResult> GetCoursesByUserId(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound("User not found");

        var courses = await _courseService.GetCoursesByUserIdAsync(userId);
        if (!courses.Any()) return NotFound("No courses found for this user");

        return Ok(courses);
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



    [HttpGet("user/{userId}/courses")]
    public async Task<IActionResult> GetUserCourses(string userId)
    {
        // Kullanıcının aldığı kursları getiriyoruz
        var courses = await _courseService.GetCoursesByUserIdAsync(userId);
        if (!courses.Any())
        {
            return NotFound("Bu kullanıcıya ait kurs bulunamadı.");
        }

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

    
    [HttpGet("GetCoursesByGroup/{groupId}")]
    public async Task<IActionResult> GetCoursesByGroup(int groupId)
    {
        var courses = await _courseService.GetCoursesByGroupAsync(groupId);
        if (courses == null || !courses.Any())
        {
            return NotFound("Gruba ait kurs bulunamadı.");
        }
        return Ok(courses);
    }
    [HttpGet("GetAllCourses")]
    [Authorize(Roles = "So,MasterAdmin")] 
    public async Task<IActionResult> GetAllCourses()
    {
        var user = await _userManager.GetUserAsync(User);
        var roles = await _userManager.GetRolesAsync(user);

        // Eğer kullanıcı So veya MasterAdmin değilse, hata döndür
        if (!roles.Contains("So") && !roles.Contains("MasterAdmin"))
        {
            return Forbid("Bu işlemi gerçekleştirmek için yetkiniz yok.");
        }

        // Kullanıcı uygun rollere sahipse tüm kursları getir
        var courses = await _courseService.GetAllCoursesAsync();
        return Ok(courses);
    }
}



