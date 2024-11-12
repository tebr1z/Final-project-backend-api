using LmsApiApp.Application.Interfaces;
using LmsApiApp.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Route("api/[controller]")]
[ApiController]
public class AttendanceController : ControllerBase
{
    private readonly IAttendanceService _attendanceService;
    private readonly UserManager<User> _userManager;

    public AttendanceController(IAttendanceService attendanceService, UserManager<User> userManager)
    {
        _attendanceService = attendanceService;
        _userManager = userManager;
    }

   
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Attendance>>> GetAttendances()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 

        var attendances = await _attendanceService.GetAllAttendancesAsync(userId);
        return Ok(attendances);
    }

    // GET: api/Attendance/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Attendance>> GetAttendance(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
        var attendance = await _attendanceService.GetAttendanceByIdAsync(id, userId);

        if (attendance == null)
        {
            return NotFound();
        }

        return Ok(attendance);
    }

   
    [HttpPost]
    public async Task<ActionResult> CreateAttendance([FromBody] int courseId)
    {
        await _attendanceService.AddAttendanceAsync(courseId);
        return CreatedAtAction(nameof(GetAttendances), new { message = "Attendance created successfully" });
    }

    // PUT: api/Attendance/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAttendance(int id, [FromBody] Attendance attendance)
    {
        await _attendanceService.UpdateAttendanceAsync(id, attendance);
        return NoContent();
    }

 
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAttendance(int id)
    {
        await _attendanceService.DeleteAttendanceAsync(id);
        return NoContent();
    }
}
