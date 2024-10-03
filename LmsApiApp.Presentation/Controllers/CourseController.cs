using AutoMapper;
using LmsApiApp.Application.Dtos.CourseDtos;
using LmsApiApp.Application.Interfaces;
using LmsApiApp.Core.Entities;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CourseController : ControllerBase
{
    private readonly ICourseServices _courseService;
    private readonly IFileUploadService _fileUploadService;
    private readonly IMapper _mapper;

    public CourseController(ICourseServices courseService, IFileUploadService fileUploadService, IMapper mapper)
    {
        _courseService = courseService;
        _fileUploadService = fileUploadService;
        _mapper = mapper;
    }

    // POST: api/Course (Course oluşturma)
    [HttpPost]
    public async Task<ActionResult> CreateCourse([FromForm] CourseDto courseDto, IFormFile mediaFile)
    {
        // Eğer bir medya dosyası yüklendiyse
        if (mediaFile != null)
        {
            var mediaUrl = await _fileUploadService.UploadFileAsync(mediaFile);
            courseDto.Img = mediaUrl; // Medya URL'sini DTO'ya ekle
        }

        // DTO'dan Course entity'sine dönüştürme
        var newCourse = _mapper.Map<Course>(courseDto);

        // CreatedDate alanını burada otomatik olarak ayarlıyoruz
        newCourse.CreatedDate = DateTime.UtcNow;

        // Kursu ekle
        await _courseService.AddCourseAsync(newCourse);

        // DTO geri döndürülüyor
        var createdCourseDto = _mapper.Map<CourseDto>(newCourse); // Entity'den DTO'ya dönüşüm
        return CreatedAtAction(nameof(GetCourse), new { id = newCourse.Id }, createdCourseDto);
    }

    // GET: api/Course/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CourseDto>> GetCourse(int id)
    {
        var course = await _courseService.GetCourseByIdAsync(id);

        if (course == null)
        {
            return NotFound();
        }

        var courseDto = _mapper.Map<CourseDto>(course);
        return Ok(courseDto);
    }
}
