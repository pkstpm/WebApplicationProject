using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApplicationProject.Areas.Identity.Data;
using WebApplicationProject.Data;
using WebApplicationProject.Models;
using WebApplicationProject.ViewModel;


namespace WebApplicationProject.Controllers
{
    public class EventController : Controller
    {
        private readonly WebApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public EventController(WebApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        public async Task<IActionResult> Index()
        {
            var events = _context.Events.ToList();

            var user = await _userManager.GetUserAsync(User); // Get the current user's 
            var viewModel = new AllEventViewModel
            {
                MyEvents = _context.Events.Where(e => e.UserID == user.Id).ToList(),
                JoinedEvents = _context.Events.Where(e => e.UserEvents.Any(ue => ue.UserID == user.Id)).ToList(),
                OtherEvents = _context.Events.Where(e => e.UserID != user.Id && !e.UserEvents.Any(ue => ue.UserID == user.Id)).ToList()
            };

            return View(viewModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEventViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                var @event = new Event
                {
                    Id = Guid.NewGuid(),
                    Title = model.Title,
                    Category = model.Category,
                    ActivityTime = model.ActivityTime,
                    ExpireTime = model.ExpireTime,
                    Location = model.Location,
                    Detail = model.Detail,
                    Contact = model.Contact,
                    IsOpen = true,
                    Capacity = model.Capacity,
                    Amount = 1,
                    UserID = user.Id,
                    UserEvents = model.UserEvents,
                    Comments = model.Comments
                };

                _context.Events.Add(@event);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var @event = await _context.Events.FindAsync(id);
            var user = await _userManager.GetUserAsync(User);
            if (@event == null && @event.UserID != user.Id)
            {
                return NotFound();
            }
            var model = new EditEventViewModel
            {
                Id = @event.Id,
                Title = @event.Title,
                Detail = @event.Detail,
                Contact = @event.Contact,
                Location = @event.Location,
                ActivityTime= @event.ActivityTime,
                ExpireTime = @event.ExpireTime,
                Capacity = @event.Capacity,
                IsOpen = @event.IsOpen,
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Join(Guid Id)
        {
            var user = await _userManager.GetUserAsync(User);
            var @event = await _context.Events.FindAsync(Id);

            var userEventIds = _context.UserEvents
                           .Where(ue => ue.UserID == user.Id)
                           .Select(ue => ue.EventID)
                           .Distinct()
                           .ToList();

            if (userEventIds.Contains(@event.Id)) {
                return Content("12345");
            }
            var @userevent = new UserEvent
            {
                UserID = user.Id,
                EventID = @event.Id,
                IsJoin = true
            };

            _context.UserEvents.Add(@userevent);
            user.UserEvents.Add(@userevent);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditEventViewModel viewmodel)
        {
            var user = await _userManager.GetUserAsync(User);
            var @event = await _context.Events.FindAsync(viewmodel.Id);
            if (ModelState.IsValid && user.Id == @event.UserID)
            {
                @event.Title = viewmodel.Title;
                @event.Detail = viewmodel.Detail;
                @event.Contact = viewmodel.Contact;
                @event.Location = viewmodel.Location;
                @event.ActivityTime = viewmodel.ActivityTime;
                @event.ExpireTime = viewmodel.ExpireTime;
                @event.Capacity = viewmodel.Capacity;

                await _context.SaveChangesAsync();

                return Content("555s");
            }

            return Content("666");
            
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var user = await _userManager.GetUserAsync(User);
            var @event = await _context.Events.FindAsync(Id);
            if (ModelState.IsValid && user.Id == @event.UserID)
            {
                _context.Events.Remove(@event);
                _context.SaveChanges();


                return Content("555s");
            }

            return Content("666");

        }

        // GET: Event/MoreDetail
        public async Task<IActionResult> MoreDetail(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(e => e.Id == id);

            if (@event == null)
            {
                return NotFound();
            }

            // Load comments associated with the event
            @event.Comments = await _context.Comments
                .Where(c => c.EventId == id)
                .Include(c => c.User)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            return View(@event);
        }

        // POST: Event/AddComment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(Guid eventId, string detail)
        {
            if (string.IsNullOrEmpty(detail))
            {
                return RedirectToAction("MoreDetail", new { id = eventId });
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound(); // Or redirect to login page if user is not authenticated
            }

            var comment = new Comment
            {
                UserID = user.Id,
                EventId = eventId,
                Detail = detail,
                CreatedAt = DateTime.UtcNow
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("MoreDetail", new { id = eventId });
        }
    }
}
