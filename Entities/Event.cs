using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backends.Entities
{
    public class Event
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

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

        [ForeignKey("CreatorId")]
        public User Creator { get; set; }

        public int CreatorId { get; set; }

        public ICollection<EventAttendee> Attendees { get; set; }

        public ICollection<Review> Reviews { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
