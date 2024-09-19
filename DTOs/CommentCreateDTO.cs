using System.ComponentModel.DataAnnotations;

public class CommentCreateDto
{
    [Required]
    public string Content { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public int EventId { get; set; }
}
