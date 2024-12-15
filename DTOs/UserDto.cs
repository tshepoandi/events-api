using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

public class UserCreateDto
{
    [Required]
    [StringLength(50)]
    public string Username { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; }

    [Required]
    [PasswordPropertyText]
    [StringLength(20)]
    public string Password { get; set; }
}
