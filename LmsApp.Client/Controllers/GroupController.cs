using Microsoft.AspNetCore.Mvc;

namespace LmsApp.Client.Controllers
{
    public class GroupController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
