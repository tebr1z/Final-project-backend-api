using LmsApiApp.DataAccess.Data;
using Microsoft.AspNetCore.Mvc;

namespace LmsApiApp.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {

        private readonly LmsApiDbContext _lmsApiContext;

        public StudentController(LmsApiDbContext lmsApiContext)
        {
            _lmsApiContext = lmsApiContext;
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_lmsApiContext.CourseStudents.ToList());
        }
    }
}
