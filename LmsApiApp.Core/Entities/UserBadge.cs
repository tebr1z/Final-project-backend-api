namespace LmsApiApp.Core.Entities
{
    public class UserBadge
    {
        public int Id { get; set; }
        public string BadgeTitle { get; set; }
        public string BadgeDescription { get; set; }
        public DateTime EarnedAt { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }

}
