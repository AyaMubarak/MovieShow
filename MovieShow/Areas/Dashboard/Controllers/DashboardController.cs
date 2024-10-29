using Microsoft.AspNetCore.Mvc;

namespace MovieShow.Areas.Dashboard.Controllers
{
    public class DashboardController : Controller
    {
        [Area("Dashboard")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
