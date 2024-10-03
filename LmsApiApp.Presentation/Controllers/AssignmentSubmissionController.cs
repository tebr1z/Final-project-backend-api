using AutoMapper;
using LmsApiApp.Application.Dtos.AssignmentDtos;
using LmsApiApp.Application.Interfaces;
using LmsApiApp.Application.Services;
using LmsApiApp.Core.Entities;
using LmsApiApp.DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class AssignmentSubmissionController : ControllerBase
{
    private readonly IAssignmentService _assignmentService;
    private readonly UserManager<User> _userManager;
    private readonly LmsApiDbContext _context;
    private readonly IMapper _mapper;
    private readonly IFileUploadService _fileUploadService;

    public AssignmentSubmissionController(IAssignmentService assignmentService, UserManager<User> userManager, LmsApiDbContext context, IMapper mapper, IFileUploadService fileUploadService)
    {
        _assignmentService = assignmentService;
        _userManager = userManager;
        _context = context;
        _mapper = mapper;
        _fileUploadService = fileUploadService;
    }
    [HttpPost("submit")]
    public async Task<IActionResult> SubmitMedia([FromForm] AssignmentSubmissionDto submissionDto, IFormFile mediaFile)
    {
        // DTO'dan gelen UserId'yi alıyoruz
        var userId = submissionDto.UserId;

        if (string.IsNullOrEmpty(userId))
        {
            return BadRequest("UserId is required.");
        }

        // UserManager ile UserId'yi kullanarak kullanıcıyı buluyoruz
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        // Kullanıcının en son gönderdiği ödevi buluyoruz
        var latestSubmission = await _context.AssignmentSubmissions
            .Where(s => s.AssignmentId == submissionDto.AssignmentId && s.UserId == userId)
            .OrderByDescending(s => s.SubmittedAt)
            .FirstOrDefaultAsync();

        // Eğer en son gönderilen ödevin puanı 50 veya daha büyükse tekrar gönderime izin vermeyin
        if (latestSubmission != null && latestSubmission.Grade.HasValue && latestSubmission.Grade >= 50)
        {
            return BadRequest("You cannot resubmit this assignment because the grade is 50 or higher.");
        }

        // Yeni ödev teslimini kaydediyoruz
        var submission = _mapper.Map<AssignmentSubmission>(submissionDto);
        submission.SubmittedAt = DateTime.UtcNow;

        // Eğer medya dosyası yüklendiyse, dosyayı yükleyip URL'sini kaydediyoruz
        if (mediaFile != null)
        {
            var mediaUrl = await _fileUploadService.UploadFileAsync(mediaFile);
            submission.MediaUrl = mediaUrl;
        }

        // Yeni gönderimi veritabanına ekliyoruz
        _context.AssignmentSubmissions.Add(submission);
        await _context.SaveChangesAsync();

        // Yanıt DTO'sunu hazırlıyoruz
        var responseDto = _mapper.Map<AssignmentSubmissionResponseDto>(submission);

        return Ok(responseDto);
    }





    // Tüm kullanıcıların Grade ortalamasını hesapla
    [HttpGet("average-grade/{assignmentId}")]
    public async Task<IActionResult> GetAverageGradeForAssignment(int assignmentId)
    {
        // İlgili ödev için tüm kullanıcıların en son gönderimlerini alıyoruz
        var latestSubmissions = await _context.AssignmentSubmissions
            .Where(s => s.AssignmentId == assignmentId)
            .GroupBy(s => s.UserId)
            .Select(g => g.OrderByDescending(s => s.SubmittedAt).FirstOrDefault())  // En son gönderimi alıyoruz
            .Where(s => s.Grade.HasValue)  // Yalnızca puan verilmiş gönderimleri alıyoruz
            .ToListAsync();

        if (!latestSubmissions.Any())
        {
            return NotFound("No submissions found for this assignment.");
        }

        // Ortalama puanı hesaplıyoruz
        var averageGrade = latestSubmissions.Average(s => s.Grade.Value);

        // Ödevin Grade alanını güncelliyoruz
        var assignment = await _context.Assignments.FindAsync(assignmentId);
        if (assignment != null)
        {
            assignment.Grade = averageGrade;
            _context.Assignments.Update(assignment);
            await _context.SaveChangesAsync();
        }

        return Ok(new { AssignmentId = assignmentId, AverageGrade = averageGrade });
    }

}
