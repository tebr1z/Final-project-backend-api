namespace LmsApiApp.Core.Entities
{
    public class TestResult
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public Test Test { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public int Score { get; set; }
        public bool IsReshuffled { get; set; }  // Indicates if the answers were reshuffled
        public DateTime CompletedAt { get; set; }
        public string? MediaUrl{ get; set; }
        public bool IsApprovedByTeacher { get; set; }
    }

}
