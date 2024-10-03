using LmsApiApp.Application.Interfaces;
using LmsApiApp.Core.Entities;
using LmsApiApp.DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LmsApiApp.Application.Implementations
{
    public class AssignmentService : IAssignmentService
    {
        private readonly UserManager<User> _userManager;  // IdentityUser yerine User kullanılmalı
        private readonly LmsApiDbContext _context;

        public AssignmentService(UserManager<User> userManager, LmsApiDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<bool> CanSubmitAssignment(User user)  // IdentityUser yerine User kullanılmalı
        {
            return await _userManager.IsInRoleAsync(user, "Student");
        }

        public async Task<bool> CanGradeAssignment(User user)  // IdentityUser yerine User kullanılmalı
        {
            return await _userManager.IsInRoleAsync(user, "Teacher") || await _userManager.IsInRoleAsync(user, "MasterAdmin");
        }

        public bool IsSubmissionOnTime(AssignmentSubmission submission, Assignment assignment)
        {
            return submission.SubmittedAt <= assignment.Deadline;
        }

        public async Task<bool> CanSetDeadlineAsync(User user)  // IdentityUser yerine User kullanılmalı
        {
            return await _userManager.IsInRoleAsync(user, "Teacher") || await _userManager.IsInRoleAsync(user, "MasterAdmin");
        }

        public async Task AddAssignmentAsync(Assignment assignment)
        {
            _context.Assignments.Add(assignment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAssignmentAsync(int id, Assignment assignment)
        {
            var existingAssignment = await _context.Assignments.FindAsync(id);
            if (existingAssignment != null)
            {
                // CreatedDate'e dokunmuyoruz, UpdatedDate'i güncelliyoruz
                assignment.CreatedAt = existingAssignment.CreatedAt;
                assignment.UpdatedAt = DateTime.UtcNow;  // Güncel tarih ve saat

                // Eğer bir medya dosyası varsa, yeni URL'yi ata
             if (!string.IsNullOrEmpty(assignment.MediaUrl))
        {
            existingAssignment.MediaUrl = assignment.MediaUrl; // Yeni medya URL'si ekleniyor
        }

                // Diğer alanları güncelliyoruz
                _context.Entry(existingAssignment).CurrentValues.SetValues(assignment);
                await _context.SaveChangesAsync();
            }
        }


        public async Task SubmitAssignmentAsync(int assignmentId, AssignmentSubmission submission)
        {
            // Ödev teslim işlemi
        }

        public async Task<Assignment> GetAssignmentByIdAsync(int id)
        {
            return await _context.Assignments
                                 .Where(a => a.Id == id)
                                 .FirstOrDefaultAsync(); // MediaUrl dahil tüm alanları çektiğinizden emin olun
        }


        public async Task<List<Assignment>> GetAssignmentsByCourseIdAsync(int courseId)
        {
            return await _context.Assignments.Where(a => a.CourseId == courseId).ToListAsync();
        }

        public async Task DeleteAssignmentAsync(int id)
        {
            var assignment = await _context.Assignments.FindAsync(id);
            if (assignment != null)
            {
                _context.Assignments.Remove(assignment);
                await _context.SaveChangesAsync();
            }
        }
    }
}
