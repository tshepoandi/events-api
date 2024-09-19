using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backends.Data;
using backends.Entities;

namespace backends.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewsController : ControllerBase
{
    private readonly BackendsDbContext _context;

    public ReviewsController(BackendsDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Review>>> GetReviews()
    {
        return await _context.Reviews.Include(r => r.User).Include(r => r.Event).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Review>> GetReview(int id)
    {
        var review = await _context.Reviews
            .Include(r => r.User)
            .Include(r => r.Event)
            .Include(r => r.Pictures)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (review == null)
        {
            return NotFound();
        }

        return review;
    }

    [HttpPost]
    public async Task<ActionResult<Review>> CreateReview(ReviewCreateDto reviewDto)
    {
        var review = new Review
        {
            Rating = reviewDto.Rating,
            Content = reviewDto.Content,
            UserId = reviewDto.UserId,
            EventId = reviewDto.EventId
        };

        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetReview), new { id = review.Id }, review);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateReview(int id, ReviewCreateDto reviewDto)
    {
        var review = await _context.Reviews.FindAsync(id);

        if (review == null)
        {
            return NotFound();
        }

        review.Rating = reviewDto.Rating;
        review.Content = reviewDto.Content;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ReviewExists(id))
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
    public async Task<IActionResult> DeleteReview(int id)
    {
        var review = await _context.Reviews.FindAsync(id);
        if (review == null)
        {
            return NotFound();
        }

        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ReviewExists(int id)
    {
        return _context.Reviews.Any(e => e.Id == id);
    }
}
