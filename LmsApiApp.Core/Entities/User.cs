using Microsoft.AspNetCore.Identity;

namespace LmsApiApp.Core.Entities
{
    public class User : IdentityUser

    {
        public int Id { get; set; } // Birincil anahtar
        public string UserName { get; set; } // Kullanıcı adı

        public string Img { get; set; } // Kullanıcı rolü (Öğretmen, Öğrenci, Mentor, vb.)
        public bool IsDeleted { get; set; } // Kullanıcı silinmiş mi?
        public DateTime? DeletedAt { get; set; } // Kullanıcının silindiği tarih
        public string DeletedBy { get; set; } // Kullanıcıyı kimin sildiğini takip eder
        public bool IsBanned { get; set; } // Kullanıcı yasaklı mı?
        public DateTime CreatedDate { get; set; } // Kullanıcının oluşturulduğu tarih
        public DateTime UpdatedDate { get; set; } // Kullanıcının güncellendiği tarih
        public int TotalScore { get; set; } // Toplam puan (devamlılık, ödevler vb. üzerinden hesaplanır)
        public int AttendancePercentage { get; set; } // Devamlılık yüzdesi
        public double ActiveHours { get; set; } // Platformda aktif geçirilen saat
        public int ForumMessagesCount { get; set; } // Foruma yazılan mesaj sayısı
        public string VideoChatStatus { get; set; } // WebRTC için video durumu ("Aktif", "Pasif")

        // Kullanıcının kazandığı rozetler
        public virtual ICollection<UserBadge> Badges { get; set; }
        public virtual ICollection<Assignment> Assignments { get; set; }
        public virtual ICollection<ForumPost> ForumPosts { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<CourseTeacher> CourseTeachers { get; set; } // Öğrettiği dersler
        public virtual ICollection<CourseStudent> CourseStudents { get; set; }
        public virtual ICollection<TestResult> TestResults { get; set; }
        public ICollection<GroupEnrollment> GroupEnrollments { get; set; }

        // Role-based control: User roles (Student, Teacher, etc.)
        public ICollection<IdentityUserRole<string>> UserRoles { get; set; }


    }
}