using System.ComponentModel.DataAnnotations;

public class EventCreateDto
{
    [Required]
    [StringLength(100)]
    public string Title { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public DateTime StartTime { get; set; }

    [Required]
    public DateTime EndTime { get; set; }

    [Required]
    [StringLength(100)]
    public string Location { get; set; }

    [Required]
    public int CreatorId { get; set; }
}
