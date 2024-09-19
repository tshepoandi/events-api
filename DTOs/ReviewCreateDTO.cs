using System.ComponentModel.DataAnnotations;

public class ReviewCreateDto
{
    [Required]
    [Range(1, 5)]
    public int Rating { get; set; }

    [Required]
    public string Content { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public int EventId { get; set; }
}
