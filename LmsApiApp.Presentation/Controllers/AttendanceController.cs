using Google;
using LmsApiApp.Application.Dtos.AssignmentDtos;
using LmsApiApp.Core.Entities;
using LmsApiApp.DataAccess.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

[Route("api/attendance")]
[ApiController]
[Authorize]
public class AttendanceController : ControllerBase
{
    private readonly LmsApiDbContext _context;

    public AttendanceController(LmsApiDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Öğrenci yoklamasını kaydetme
    /// </summary>
    [HttpPost("mark")]
    public async Task<IActionResult> MarkAttendance([FromBody] MarkAttendanceDto model)
    {
        var student = await _context.Users.FindAsync(model.StudentId);
        if (student == null)
            return NotFound("Student not found.");

        var course = await _context.Courses.FindAsync(model.CourseId);
        if (course == null)
            return NotFound("Course not found.");

        var attendance = new Attendance
        {
            StudentId = model.StudentId,
            CourseId = model.CourseId,
            Date = DateTime.UtcNow,
            IsPresent = model.IsPresent,
            ExcuseDocument = null
        };

        _context.Attendances.Add(attendance);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Attendance recorded successfully." });
    }

    /// <summary>
    /// Belirli bir kursun tüm yoklamalarını getirir
    /// </summary>
    [HttpGet("course/{courseId}")]
    public async Task<IActionResult> GetAttendanceByCourse(int courseId)
    {
        var attendanceRecords = await _context.Attendances
            .Where(a => a.CourseId == courseId)
            .Include(a => a.Student)
            .ToListAsync();

        if (!attendanceRecords.Any())
            return NotFound("No attendance records found for this course.");

        return Ok(attendanceRecords.Select(a => new
        {
            a.Student.FullName,
            a.Date,
            a.IsPresent,
            a.ExcuseDocument
        }));
    }

    /// <summary>
    /// Belirli bir öğrencinin devam durumunu getirir
    /// </summary>
    [HttpGet("student/{studentId}")]
    public async Task<IActionResult> GetStudentAttendance(int studentId)
    {
        var attendanceRecords = await _context.Attendances
            .Where(a => a.StudentId == studentId)
            .ToListAsync();

        if (!attendanceRecords.Any())
            return NotFound("No attendance records found for this student.");

        return Ok(attendanceRecords.Select(a => new
        {
            a.Date,
            a.IsPresent,
            a.ExcuseDocument
        }));
    }

    /// <summary>
    /// Mazaret belgesi yükleme
    /// </summary>
    [HttpPost("upload-excuse/{attendanceId}")]
    public async Task<IActionResult> UploadExcuseDocument(int attendanceId, [FromForm] IFormFile file)
    {
        var attendance = await _context.Attendances.FindAsync(attendanceId);
        if (attendance == null)
            return NotFound("Attendance record not found.");

        if (file == null || file.Length == 0)
            return BadRequest("Invalid file.");

        // Dosya adını oluştur ve kaydet
        string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "excuses");
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        string filePath = Path.Combine(uploadsFolder, $"{Guid.NewGuid()}_{file.FileName}");
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        attendance.ExcuseDocument = filePath;
        await _context.SaveChangesAsync();

        return Ok(new { message = "Excuse document uploaded successfully.", filePath });
    }
}
