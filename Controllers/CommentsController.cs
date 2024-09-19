using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backends.Data;
using backends.Entities;

namespace backends.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly BackendsDbContext _context;

    public CommentsController(BackendsDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Comment>>> GetComments()
    {
        return await _context.Comments.Include(c => c.User).Include(c => c.Event).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Comment>> GetComment(int id)
    {
        var comment = await _context.Comments
            .Include(c => c.User)
            .Include(c => c.Event)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (comment == null)
        {
            return NotFound();
        }

        return comment;
    }

    [HttpPost]
    public async Task<ActionResult<Comment>> CreateComment(CommentCreateDto commentDto)
    {
        var comment = new Comment
        {
            Content = commentDto.Content,
            UserId = commentDto.UserId,
            EventId = commentDto.EventId
        };

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetComment), new { id = comment.Id }, comment);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateComment(int id, CommentCreateDto commentDto)
    {
        var comment = await _context.Comments.FindAsync(id);

        if (comment == null)
        {
            return NotFound();
        }

        comment.Content = commentDto.Content;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CommentExists(id))
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
    public async Task<IActionResult> DeleteComment(int id)
    {
        var comment = await _context.Comments.FindAsync(id);
        if (comment == null)
        {
            return NotFound();
        }

        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool CommentExists(int id)
    {
        return _context.Comments.Any(e => e.Id == id);
    }
}
