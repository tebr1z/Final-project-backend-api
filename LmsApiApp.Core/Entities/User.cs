using Microsoft.AspNetCore.Identity;

namespace LmsApiApp.Core.Entities
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public string LastName { get; set; }
        public string? Img { get; set; } = "default.png";
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
        public bool? IsBanned { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }
        public int TotalScore { get; set; } = 0;
        public int AttendancePercentage { get; set; } = 0;
        public double ActiveHours { get; set; } = 0.0;
        public int ForumMessagesCount { get; set; } = 0;
        public string VideoChatStatus { get; set; } = "Inactive";
        public string? ResetPasswordOtp { get; set; }
        public DateTime? OtpExpiryTime { get; set; }

        // Yeni Alanlar
        public string? Role { get; set; } // Görev/Vazife (Öğrenci, Öğretmen, Mentor)
        public string? Skills { get; set; } // Yetkinlikler (Ör: Frontend Dev, Backend Dev)
        public string? Gender { get; set; } // Cinsiyet
        public DateTime? BirthDate { get; set; } // Doğum Tarihi
        public string? InstagramLink { get; set; }
        public string? LinkedinLink { get; set; }
        public string? GithubLink { get; set; }
        public string? BehanceLink { get; set; }

        // Kullanıcının kazandığı rozetler
        public virtual ICollection<UserBadge> Badges { get; set; }
        public virtual ICollection<Assignment> Assignments { get; set; }
        public virtual ICollection<ForumPost> ForumPosts { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<CourseTeacher> CourseTeachers { get; set; }
        public virtual ICollection<CourseStudent> CourseStudents { get; set; }
        public virtual ICollection<TestResult> TestResults { get; set; }
        public ICollection<GroupEnrollment> GroupEnrollments { get; set; }

        public ICollection<IdentityUserRole<string>> UserRoles { get; set; }

        public DateTime LastActive { get; set; } = DateTime.Now;
        public bool IsOnline { get; set; } = false;
    }
}
