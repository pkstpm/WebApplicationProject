using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Reflection.Metadata;
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
            if (!_signInManager.IsSignedIn(User))
            {
                var viewModels = new AllEventViewModel
                {
                    OtherEvents = await _context.Events.OrderByDescending(e => e.IsOpen).ToListAsync()
                };

                return View(viewModels);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            ViewBag.AlertMessage = TempData["JoinAlert"];
            var myEventsQuery = _context.Events.Where(e => e.UserID == user.Id).OrderByDescending(e => e.IsOpen);
            var joinedEventsQuery = _context.Events.Where(e => e.UserEvents.Any(ue => ue.UserID == user.Id && ue.IsJoin)).OrderByDescending(e => e.IsOpen);
            var otherEventsQuery = _context.Events.Where(e => e.UserID != user.Id && !e.UserEvents.Any(ue => ue.UserID == user.Id && ue.IsJoin)).OrderByDescending(e => e.IsOpen);

            var viewModel = new AllEventViewModel
            {
                MyEvents = await myEventsQuery.ToListAsync(),
                JoinedEvents = await joinedEventsQuery.ToListAsync(),
                OtherEvents = await otherEventsQuery.ToListAsync()
            };

            return View(viewModel);
        }

        [HttpGet("/event/category/{categoryName}")]
        public async Task<IActionResult> Category(string categoryName)
        {
            Category category;
            if (!Enum.TryParse(categoryName, true, out category))
            {

                return BadRequest("Invalid category name");
            }
            if (_signInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var viewModel = new AllEventViewModel
                    {
                        MyEvents = [.. _context.Events.Where(e => e.Category == category).Where(e => e.UserID == user.Id).OrderByDescending(e => e.IsOpen)],
                        JoinedEvents = [.. _context.Events.Where(e => e.Category == category).Where(e => e.UserEvents.Any(ue => ue.UserID == user.Id && ue.IsJoin == true)).OrderByDescending(e => e.IsOpen)],
                        OtherEvents = [.. _context.Events.Where(e => e.Category == category).Where(e => e.UserID != user.Id && !e.UserEvents.Any(ue => ue.UserID == user.Id && ue.IsJoin == true)).OrderByDescending(e => e.IsOpen)]
                    };

                    return View("Index", viewModel);
                }
                throw new Exception("Can't find User");
            }

            var viewModels = new AllEventViewModel
            {
                OtherEvents = [.. _context.Events.Where(e => e.Category == category).OrderByDescending(e => e.IsOpen)]
            };

            return View("Index", viewModels);
        }

        public async Task<IActionResult> Detail(Guid Id)
        {
            if (TempData["CreateAlert"] != null)
            {
                ViewBag.AlertMessage = TempData["CreateAlert"];

            }
            if (TempData["JoinAlert"] != null)
            {
                ViewBag.AlertMessage = TempData["JoinAlert"];
            }

            var @event = await _context.Events.Include(e => e.User).Include(e => e.UserEvents).ThenInclude(ue => ue.User).FirstOrDefaultAsync(e => e.Id == Id);

            if (@event != null)
            {
                @event.Comments = await _context.Comments
                .Where(c => c.EventId == Id)
                .Include(c => c.User)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

                return View(@event);
            }
            throw new Exception("Can't find Event");
        }

        public IActionResult Create()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return View();
            }
            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEventViewModel model)
        {
            if (_signInManager.IsSignedIn(User))
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);
                    if (user != null)
                    {
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
                            UserEvents = [],
                            Comments = []
                        };

                        _context.Events.Add(@event);
                        await _context.SaveChangesAsync();
                        TempData["CreateAlert"] = "Success "+ @event.Title +" Create!!";

                        return RedirectToAction(nameof(Detail), new { id = @event.Id });
                    }
                    throw new Exception("Can't find User");
                }

                throw new Exception("Model is wrong");
            }

            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            if (_signInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                { 
                    var @event = await _context.Events.FindAsync(id);

                    if (@event == null || @event.UserID != user.Id)
                    {
                        throw new Exception("Dont found Event or this is not your event");
                    }
                    var model = new EditEventViewModel
                    {
                        Id = @event.Id,
                        Title = @event.Title,
                        Detail = @event.Detail,
                        Contact = @event.Contact,
                        Location = @event.Location,
                        ActivityTime = @event.ActivityTime,
                        ExpireTime = @event.ExpireTime,
                        Capacity = @event.Capacity,
                        IsOpen = @event.IsOpen
                    };
                    return View(model);
                }
                throw new Exception("Can't find User");
            }

            return RedirectToPage("/Account/Login", new { area = "Identity" });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditEventViewModel viewmodel)
        {
            if (_signInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var @event = await _context.Events.FindAsync(viewmodel.Id);
                    if (@event != null)
                    {
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
                    }
                    throw new Exception("Can't find Event");
                }
                throw new Exception("Can't find User");
            }
            return RedirectToPage("/Account/Login", new { area = "Identity" });

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Close(Guid Id)
        {
            if (_signInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var @event = await _context.Events.FindAsync(Id);
                    if (@event != null)
                    {
                        if (user.Id == @event.UserID)
                        {
                            if (@event.IsOpen == true)
                            { 
                                @event.IsOpen = false;

                                await _context.SaveChangesAsync();

                                return RedirectToAction("Index", "Event", new { id = Id });
                            }
                            throw new Exception("This event is already close");
                        }
                        throw new Exception("This is not your event");
                    }
                    throw new Exception("Can't find Event");
                }
                throw new Exception("Can't find User");
            }
            return RedirectToPage("/Account/Login", new { area = "Identity" });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Join(Guid Id)
        {
            var user = await _userManager.GetUserAsync(User);
            var @event = await _context.Events.FindAsync(Id);

            var userEvent = _context.UserEvents.FirstOrDefault(ue => ue.UserID == user.Id && ue.EventID == @event.Id);

            if (userEvent != null && userEvent.EventID == @event.Id && @event.Capacity > @event.Amount)
            {
                userEvent.IsJoin = true;

                @event.Amount += 1;

                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            if (@event.IsOpen != false)
            {
                if (@event.Amount < @event.Capacity)
                {
                    @event.Amount += 1;
                    var userevent = new UserEvent
                    {
                        UserID = user.Id,
                        EventID = @event.Id,
                        IsJoin = true
                    };

                    _context.UserEvents.Add(userevent); 
                    await _context.SaveChangesAsync();

                    @event.UserEvents.Add(userevent);
                    _context.Update(@event);

                    user.UserEvents.Add(userevent);
                    await _userManager.UpdateAsync(user);
                    TempData["JoinAlert"] = "Join "+ @event.Title +" Success!!" ;

                    return RedirectToAction(nameof(Index));
                }
                throw new Exception("This Event is full");
            }
            throw new Exception("This Event is close");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Quit(Guid Id)
        {
            var user = await _userManager.GetUserAsync(User);
            var @event = await _context.Events.FindAsync(Id);

            var userEvent = _context.UserEvents.FirstOrDefault(ue => ue.UserID == user.Id && ue.EventID == @event.Id);

            if (userEvent != null)
            {
                userEvent.IsJoin = false;

                @event.Amount -= 1;

                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            throw new Exception("This Event is close");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var user = await _userManager.GetUserAsync(User);
            var @event = await _context.Events.FindAsync(Id);
            if (user.Id == @event.UserID)
            {
                _context.Events.Remove(@event);
                var userevents = await _context.UserEvents.Where(ue => ue.EventID == @event.Id).ToListAsync();
                _context.UserEvents.RemoveRange(userevents);
                var comments = await _context.Comments.Where(c => c.EventId == Id).ToListAsync();
                _context.Comments.RemoveRange(comments);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }

            throw new Exception("This is not your event");

        }

        // POST: Event/AddComment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(Guid eventId, string detail)
        {
            var @event = await _context.Events.Include(e => e.User).Include(e => e.UserEvents).ThenInclude(ue => ue.User).FirstOrDefaultAsync(e => e.Id == eventId);
            var user = await _userManager.GetUserAsync(User);

            if (@event == null || user == null)
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(detail))
            {
                return RedirectToAction("Index", "Event", new { id = eventId });
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

            return RedirectToAction("Index", "Event", new { id = eventId });
        }
    }
}
