using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace backends.Entities
{
    [Table("users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public required string Username { get; set; }
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public required string Email { get; set; }

        public ICollection<Event> CreatedEvents {get; set;}
        public ICollection<EventAttendee> AttendingEvents {get; set;}
        public ICollection<Review> Reviews {get; set;}
        public ICollection<Comment> Comments {get;set;}
    }
}
