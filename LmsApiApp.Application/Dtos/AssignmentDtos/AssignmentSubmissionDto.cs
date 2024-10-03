using System;

namespace LmsApiApp.Application.Dtos.AssignmentDtos
{
    public class AssignmentSubmissionDto
    {
        public int AssignmentId { get; set; } // Ödev Id'si
        public string SubmissionContent { get; set; } // Öğrenci tarafından gönderilen içerik (metin)

        // Nullable alanlar
        public string? MediaUrl { get; set; } // Tek bir medya URL'si

        public string UserId { get; set; }

    }
}
