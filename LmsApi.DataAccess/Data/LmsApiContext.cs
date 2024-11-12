using LmsApiApp.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;


namespace LmsApiApp.DataAccess.Data
{
    public class LmsApiDbContext : IdentityDbContext<User>
    {
        public LmsApiDbContext(DbContextOptions<LmsApiDbContext> options) : base(options) { }

       
     

        public DbSet<CourseTeacher> CourseTeachers { get; set; }  
        public DbSet<CourseStudent> CourseStudents { get; set; }
        public DbSet<Course> Courses { get; set; }

     
        public DbSet<Assignment> Assignments { get; set; }

        public DbSet<AssignmentSubmission> AssignmentSubmissions { get; set; }

        public DbSet<Test> Tests { get; set; }

        public DbSet<Question> Questions { get; set; }
        public DbSet<TestResult> TestResults { get; set; }


        public DbSet<Attendance> Attendances { get; set; }


        public DbSet<ChatRoom> ChatRooms { get; set; }


        public DbSet<ChatMessage> ChatMessages { get; set; }

      
        public DbSet<UserBadge> Badges { get; set; }

  
        public DbSet<ForumThread> ForumThreads { get; set; }


        public DbSet<ForumPost> ForumPosts { get; set; }


        public DbSet<UserActivity> UserActivities { get; set; }
        public DbSet<Message> messages { get; set; }



        public DbSet<Settings> Settings { get; set; }
        public DbSet<Group> Groups { get; set; }

        public DbSet<GroupEnrollment> GroupEnrollments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // TestResult ve Test ilişkisi
            modelBuilder.Entity<TestResult>()
                .HasOne(tr => tr.Test)
                .WithMany(t => t.TestResults)
                .HasForeignKey(tr => tr.TestId)
                .OnDelete(DeleteBehavior.Restrict);

            // TestResult ve User (IdentityUser) ilişkisi
            modelBuilder.Entity<TestResult>()
                .HasOne(tr => tr.User)
                .WithMany(u => u.TestResults)
                .HasForeignKey(tr => tr.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // ForumPost ve ForumThread ilişkisi
            modelBuilder.Entity<ForumPost>()
                .HasOne(fp => fp.Thread)
                .WithMany(ft => ft.ForumPosts)
                .HasForeignKey(fp => fp.ThreadId)
                .OnDelete(DeleteBehavior.Restrict);

            // ForumPost ve User ilişkisi
            modelBuilder.Entity<ForumPost>()
                .HasOne(fp => fp.User)
                .WithMany(u => u.ForumPosts)
                .HasForeignKey(fp => fp.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // CourseStudent ve Course ilişkisi
            modelBuilder.Entity<CourseStudent>()
                .HasKey(cs => new { cs.CourseId, cs.UserId });

            modelBuilder.Entity<CourseStudent>()
                .HasOne(cs => cs.Course)
                .WithMany(c => c.CourseStudents)
                .HasForeignKey(cs => cs.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CourseStudent>()
                .HasOne(cs => cs.Student)
                .WithMany(u => u.CourseStudents)
                .HasForeignKey(cs => cs.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // CourseTeacher ve Course ilişkisi
            modelBuilder.Entity<CourseTeacher>()
                .HasKey(ct => new { ct.CourseId, ct.UserId });

            modelBuilder.Entity<CourseTeacher>()
                .HasOne(ct => ct.Course)
                .WithMany(c => c.CourseTeachers)
                .HasForeignKey(ct => ct.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CourseTeacher>()
                .HasOne(ct => ct.Teacher)
                .WithMany(u => u.CourseTeachers)
                .HasForeignKey(ct => ct.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Message tablosu ve ilişkiler
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany()  // Kullanıcıların mesaj koleksiyonu yok
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);  // Restrict, cascade'i devre dışı bırakır.

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany()  // Kullanıcıların mesaj koleksiyonu yok
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

        }

    }

}
