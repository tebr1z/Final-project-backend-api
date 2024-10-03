namespace LmsApiApp.Core.Entities
{
    public class UserActivity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime LogoutTime { get; set; }
        public int TotalHoursActive { get; set; }  // Calculated from login/logout
        public int ForumPosts { get; set; }
    }

}
