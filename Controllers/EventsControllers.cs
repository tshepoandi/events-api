using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backends.Data;
using backends.Entities;

namespace backends.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly BackendsDbContext _context;

        public EventsController(BackendsDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            return await _context.Events.Include(e => e.Creator).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            var @event =
                await _context
                    .Events
                    .Include(e => e.Creator)
                    .Include(e => e.Attendees)
                    .Include(e => e.Reviews)
                    .Include(e => e.Comments)
                    .FirstOrDefaultAsync(e => e.Id == id);

            if (@event == null)
            {
                return NotFound();
            }

            return @event;
        }

        [HttpPost]
        public async Task<ActionResult<Event>>
        CreateEvent(EventCreateDto eventDto)
        {
            var @event =
                new Event {
                    Title = eventDto.Title,
                    Description = eventDto.Description,
                    StartTime = eventDto.StartTime,
                    EndTime = eventDto.EndTime,
                    Location = eventDto.Location,
                    CreatorId = eventDto.CreatorId
                };

            _context.Events.Add (@event);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvent),
            new { id = @event.Id },
            @event);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult>
        UpdateEvent(int id, EventCreateDto eventDto)
        {
            var @event = await _context.Events.FindAsync(id);

            if (@event == null)
            {
                return NotFound();
            }

            @event.Title = eventDto.Title;
            @event.Description = eventDto.Description;
            @event.StartTime = eventDto.StartTime;
            @event.EndTime = eventDto.EndTime;
            @event.Location = eventDto.Location;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            _context.Events.Remove (@event);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("{id}/attend")]
        public async Task<IActionResult> AttendEvent(int id, int userId)
        {
            var @event = await _context.Events.FindAsync(id);
            var user = await _context.Users.FindAsync(userId);

            if (@event == null || user == null)
            {
                return NotFound();
            }

            var attendance =
                new EventAttendee { UserId = userId, EventId = id };

            _context.EventAttendees.Add (attendance);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}/attend")]
        public async Task<IActionResult> UnattendEvent(int id, int userId)
        {
            var attendance =
                await _context
                    .EventAttendees
                    .FirstOrDefaultAsync(ea =>
                        ea.EventId == id && ea.UserId == userId);

            if (attendance == null)
            {
                return NotFound();
            }

            _context.EventAttendees.Remove (attendance);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
