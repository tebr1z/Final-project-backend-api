using LmsApiApp.Core.Entities;

public interface IAttendanceService
{
    Task<IEnumerable<Attendance>> GetAllAttendancesAsync(string userId);
    Task<Attendance> GetAttendanceByIdAsync(int id, string userId);
    Task AddAttendanceAsync(int courseId);  // Sadece CourseId alıyor
    Task UpdateAttendanceAsync(int id, Attendance attendance);
    Task DeleteAttendanceAsync(int id);
}
