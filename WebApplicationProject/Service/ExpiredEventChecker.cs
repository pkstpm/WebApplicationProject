using Microsoft.EntityFrameworkCore;
using WebApplicationProject.Data;

namespace WebApplicationProject.Service
{
    public class ExpiredEventChecker : BackgroundService
    {
        private readonly IServiceProvider _services;

        public ExpiredEventChecker(IServiceProvider services)
        {
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _services.CreateScope())
                {
                    var _context = scope.ServiceProvider.GetRequiredService<WebApplicationDbContext>(); // Replace YourDbContext with your actual DbContext class
                    var eventsToClose = await _context.Events
                        .Where(e => e.ExpireTime <= DateTime.Now && e.IsOpen)
                        .ToListAsync();

                    foreach (var ev in eventsToClose)
                    {
                        ev.IsOpen = false;
                    }

                    await _context.SaveChangesAsync();
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
