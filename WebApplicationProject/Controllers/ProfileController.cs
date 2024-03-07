using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Editprofile()
        {
            return View();
        }
    }
}
