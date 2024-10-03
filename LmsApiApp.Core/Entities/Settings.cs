namespace LmsApiApp.Core.Entities
{
    public class Settings
    {
        public int Id { get; set; }
        public string? Key { get; set; }
            public string? Value { get; set; }
        public decimal AttendancePercentage { get; set; }  // Percentage impact of attendance on final grade
        public decimal HomeworkPercentage { get; set; }    // Percentage impact of homework
        public decimal TestPercentage { get; set; }        // Percentage impact of tests
        public decimal MiniProjectPercentage { get; set; } // Percentage impact of mini projects
    }

}
