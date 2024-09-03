using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backends.Data;
using backends.Entities;

namespace backends.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly BackendsDbContext _context;

        public UsersController(BackendsDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            _context.Users.Add (user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUsers),
            "Users",
            new { id = user.Id },
            user);
        }
    }
}
