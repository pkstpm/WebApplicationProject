using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using WebApplicationProject.Areas.Identity.Data;
using WebApplicationProject.Data;
using WebApplicationProject.ViewModel;

namespace WebApplicationProject.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly WebApplicationDbContext _context;

        public UserController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            WebApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (_signInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound();
                }

                var viewModel = new ViewProfileViewModel
                {
                    User = user,
                    MyEvents = [.. _context.Events.Where(e => e.UserID == user.Id).OrderByDescending(e => e.IsOpen)],
                    JoinedEvents = [.. _context.Events.Where(e => e.UserEvents.Any(ue => ue.UserID == user.Id && ue.IsJoin == true)).OrderByDescending(e => e.IsOpen)]
                };

                return View(viewModel);
            }
            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }

        public async Task<IActionResult> Edit() {
            if (_signInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null) {
                    return NotFound();
                }
                var viewmodel = new EditProfileViewModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber
                };
                return View(viewmodel);
            }
            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProfileViewModel viewmodel) {
            if (_signInManager.IsSignedIn(User))
            {
                if (ModelState.IsValid)
                { 
                    var user = await _userManager.GetUserAsync(User);
                    user.FirstName = viewmodel.FirstName;
                    user.LastName = viewmodel.LastName;
                    user.Email = viewmodel.Email;
                    user.PhoneNumber = viewmodel.PhoneNumber;
                    user.UserName = viewmodel.Email;

                    await _userManager.UpdateAsync(user);

                    return RedirectToAction("Index");

                }
            }
            return RedirectToAction("Index");
        }

        public IActionResult ChangePassword() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel viewmodel)
        {
            if (_signInManager.IsSignedIn(User))
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);

                    var changePasswordResult = await _userManager.ChangePasswordAsync(user, viewmodel.OldPassword, viewmodel.NewPassword);

                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }
    }

}
