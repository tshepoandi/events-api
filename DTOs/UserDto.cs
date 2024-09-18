using System.ComponentModel.DataAnnotations;

public class UserCreateDto
{
    [Required]
    [StringLength(50)]
    public string Username { get; set; }

    // [Required]
    // [StringLength(100)]
    // public string Password { get; set; }
    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; }
}
