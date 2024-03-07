using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class MaineventController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
