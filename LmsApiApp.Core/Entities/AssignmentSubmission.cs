using System;
using Microsoft.AspNetCore.Identity;

namespace LmsApiApp.Core.Entities
{
    public class AssignmentSubmission
    {
        public int Id { get; set; }
        public int AssignmentId { get; set; } // Bağlı olduğu ödev
        public Assignment Assignment { get; set; }

        public string SubmissionContent { get; set; } // Gönderilen içerik (metin, PDF, video linki vb.)
        public string MediaUrl { get; set; } // Gönderilen medya dosyası

        public string UserId { get; set; } // IdentityUser'dan gelen kullanıcı kimliği
        public User User { get; set; } // Kullanıcı kimliği (rolü "Student" olan)

        public DateTime SubmittedAt { get; set; } // Gönderim zamanı
        public double? Grade { get; set; }
        public string? TeacherFeedback { get; set; }
    }
}
