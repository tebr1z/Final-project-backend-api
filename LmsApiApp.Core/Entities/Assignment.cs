namespace LmsApiApp.Core.Entities
{
    public class Assignment
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string MediaUrl { get; set; }
        // Course ile ilişki
        public int CourseId { get; set; }
        public Course Course { get; set; }

        // IdentityUser'dan gelen TeacherId yerine UserId kullanıyoruz
        public string UserId { get; set; }  // Kullanıcı kimliği

        // İsteğe bağlı olarak User objesini ekleyebilirsiniz
        public User User { get; set; }

        public DateTime Deadline { get; set; }

        // Not (Öğretmen tarafından verilir)
        public double? Grade { get; set; }

        // Oluşturulma ve güncellenme tarihleri
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Öğrenci geri bildirim aldıktan sonra yeniden gönderdi mi?
        public bool IsResubmitted { get; set; }
        public virtual ICollection<CourseStudent> CourseStudents { get; set; }  // Öğrencinin aldığı kurslar
        public virtual ICollection<CourseTeacher> CourseTeachers { get; set; }
        public virtual ICollection<AssignmentSubmission> Submissions { get; set; }
    }


}
