﻿using AutoMapper;
using LmsApiApp.Application.Dtos.AssignmentDtos;
using LmsApiApp.Application.Interfaces;
using LmsApiApp.Core.Entities;
using LmsApiApp.DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LmsApiApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssignmentController : ControllerBase
    {
        private readonly IAssignmentService _assignmentService;
        private readonly UserManager<User> _userManager;  
        private readonly IMapper _mapper;
        private readonly IFileUploadService _fileUploadService;
        private readonly LmsApiDbContext _context;

        public AssignmentController(IAssignmentService assignmentService, UserManager<User> userManager, IMapper mapper, IFileUploadService fileUploadService, LmsApiDbContext context)
        {
            _assignmentService = assignmentService;
            _userManager = userManager; 
            _mapper = mapper;
            _fileUploadService = fileUploadService;
            _context = context;
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateAssignment([FromForm] AssignmentDto assignmentDto, IFormFile mediaFile)
        {
         
            var userId = assignmentDto.UserId;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("UserId is required.");
            }

           
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

         

          
            var assignment = _mapper.Map<Assignment>(assignmentDto);

         
            assignment.UserId = userId;

            
            assignment.CreatedAt = DateTime.UtcNow;  
            assignment.UpdatedAt = DateTime.UtcNow; 

         
            if (mediaFile != null)
            {
           
                var mediaUrl = await _fileUploadService.UploadFileAsync(mediaFile);

               
                assignment.MediaUrl = mediaUrl;
            }

        
            await _assignmentService.AddAssignmentAsync(assignment);

            return Ok("Assignment created successfully.");
        }


        [HttpGet("course/{courseId}")]
        public async Task<IActionResult> GetAssignmentsByCourse(int courseId)
        {
            var assignments = await _assignmentService.GetAssignmentsByCourseIdAsync(courseId);

            if (assignments == null || assignments.Count == 0)
            {
                return NotFound("No assignments found for this course.");
            }

         
            var assignmentDtos = _mapper.Map<List<AssignmentDto>>(assignments);
            return Ok(assignmentDtos);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateAssignment(int id, [FromForm] UpdateAssignmentDto assignmentDto, IFormFile mediaFile)
        {
            //var user = await _userManager.GetUserAsync(User);
            //if (user == null)
            //{
            //    return Unauthorized("User not authenticated.");
            //}
            //if (!await _assignmentService.CanSetDeadlineAsync(user))
            //{
            //    return Forbid("Only teachers and master admins can update assignments.");
            //}

            var existingAssignment = await _assignmentService.GetAssignmentByIdAsync(id);
            if (existingAssignment == null)
            {
                return NotFound("Assignment not found.");
            }

 
            var oldMediaUrl = existingAssignment.MediaUrl;


            if (mediaFile != null)
            {
    
                if (!string.IsNullOrEmpty(oldMediaUrl))
                {
                    await _fileUploadService.DeleteFileAsync(oldMediaUrl); 
                }

                var newMediaUrl = await _fileUploadService.UploadFileAsync(mediaFile);
                assignmentDto.MediaUrl = newMediaUrl; 
            }
            else
            {
              
                assignmentDto.MediaUrl = oldMediaUrl;
            }

      
            var assignment = _mapper.Map<Assignment>(assignmentDto);
            assignment.Id = id;
            assignment.CreatedAt = existingAssignment.CreatedAt; // CreatedAt'e dokunulmuyor
            assignment.UpdatedAt = DateTime.UtcNow; // UpdatedAt güncelleniyor

        
            await _assignmentService.UpdateAssignmentAsync(id, assignment);
            return Ok("Assignment updated successfully.");
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAssignment(int id)
        {
            //var user = await _userManager.GetUserAsync(User);

            //if (!await _assignmentService.CanSetDeadlineAsync(user))
            //{
            //    return Forbid("Only teachers and master admins can delete assignments.");
            //}

            var assignment = await _assignmentService.GetAssignmentByIdAsync(id);
            if (assignment == null)
            {
                return NotFound("Assignment not found.");
            }

            await _fileUploadService.DeleteFileAsync(assignment.MediaUrl);
            await _assignmentService.DeleteAssignmentAsync(id);
            return Ok("Assignment deleted successfully.");
        }

       [HttpPost("grade/{submissionId}")]
public async Task<IActionResult> GiveGrade(int submissionId, [FromBody] TeacherGradeDto gradeDto)
{
   
    var submission = await _context.AssignmentSubmissions
        .Include(s => s.Assignment)
        .FirstOrDefaultAsync(s => s.Id == submissionId);

    if (submission == null)
    {
        return NotFound("Submission not found.");
    }

    // Grade'in geçerli olup olmadığını kontrol edin (0-100 arası)
    if (gradeDto.Grade < 0 || gradeDto.Grade > 100)
    {
        return BadRequest("Grade must be between 0 and 100.");
    }

    //// Öğretmenin yetkili olup olmadığını kontrol edin
    //var user = await _userManager.GetUserAsync(User);
    //if (!await _userManager.IsInRoleAsync(user, "Teacher"))
    //{
    //    return Forbid("Only teachers can give grades.");
    //}

    // Submission'a grade ve öğretmen geri bildirimini ekliyoruz
    submission.Grade = gradeDto.Grade;
    submission.TeacherFeedback = gradeDto.TeacherFeedback;  // Öğretmen geri bildirimi
    submission.SubmittedAt = DateTime.UtcNow;  // Geri bildirim verildiği zaman

    // Submission güncelleniyor
    _context.AssignmentSubmissions.Update(submission);
    await _context.SaveChangesAsync();

    // Ödev için tüm submission'ların ortalama grade'ini hesaplıyoruz
    var allSubmissions = await _context.AssignmentSubmissions
        .Where(s => s.AssignmentId == submission.AssignmentId && s.Grade.HasValue)
        .ToListAsync();

    if (allSubmissions.Any())
    {
        // Ortalamayı hesapla
        var averageGrade = allSubmissions.Average(s => s.Grade.Value);

       
        submission.Assignment.Grade = averageGrade;

        _context.Assignments.Update(submission.Assignment);
        await _context.SaveChangesAsync();
    }

    return Ok(new 
    { 
        submissionId = submission.Id, 
        grade = submission.Grade, 
        feedback = submission.TeacherFeedback, 
        averageGrade = submission.Assignment.Grade 
    });
}




    }
}
