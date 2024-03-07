using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class SignupController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
