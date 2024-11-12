using Microsoft.AspNetCore.Identity;

namespace LmsApiApp.Core.Entities
{
    public class User : IdentityUser

    {
   
    
        public string FullName { get; set; }
        public string LastName { get; set; }

        public string? Img { get; set; } // Kullanıcı rolü (Öğretmen, Öğrenci, Mentor, vb.)
        public bool IsDeleted { get; set; } = false; // Varsayılan değeri false olarak ayarladık
        public DateTime? DeletedAt { get; set; } // Nullable
        public string? DeletedBy { get; set; } // Nullable
        public bool? IsBanned { get; set; } = false; // Varsayılan değeri false
        public DateTime CreatedDate { get; set; } = DateTime.Now; // Varsayılan değer
        public DateTime? UpdatedDate { get; set; } 
        public int TotalScore { get; set; } = 0; // Varsayılan değeri 0
        public int AttendancePercentage { get; set; } = 0; // Varsayılan değeri 0
        public double ActiveHours { get; set; } = 0.0; // Varsayılan değeri 0.0
        public int ForumMessagesCount { get; set; } = 0; // Varsayılan değeri 0
        public string VideoChatStatus { get; set; } = "Inactive"; // Varsayılan değer
        public string? ResetPasswordOtp { get; set; }
        public DateTime? OtpExpiryTime { get; set; }

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

        public DateTime LastActive { get; set; } = DateTime.Now;  // Kullanıcının son aktif zamanı
        public bool IsOnline { get; set; } = false;
    }
}