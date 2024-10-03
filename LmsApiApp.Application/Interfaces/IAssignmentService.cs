using LmsApiApp.Core.Entities;  // Kendi User sınıfınızı kullanın
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace LmsApiApp.Application.Interfaces
{
    public interface IAssignmentService
    {
        Task<bool> CanSubmitAssignment(User user);  // IdentityUser yerine User kullanılmalı
        Task<bool> CanGradeAssignment(User user);  // IdentityUser yerine User kullanılmalı
        Task<bool> CanSetDeadlineAsync(User user);  // IdentityUser yerine User kullanılmalı
        bool IsSubmissionOnTime(AssignmentSubmission submission, Assignment assignment);

        Task AddAssignmentAsync(Assignment assignment);
        Task UpdateAssignmentAsync(int id, Assignment assignment);
        Task SubmitAssignmentAsync(int assignmentId, AssignmentSubmission submission);

        Task<Assignment> GetAssignmentByIdAsync(int id);
        Task<List<Assignment>> GetAssignmentsByCourseIdAsync(int courseId);
        Task DeleteAssignmentAsync(int id);
    }
}
