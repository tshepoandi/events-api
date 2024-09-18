using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backends.Entities
{
    [Table("event_attendees")]
    public class EventAttendee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public int UserId { get; set; }

        [ForeignKey("EventId")]
        public Event Event { get; set; }

        public int EventId { get; set; }

        public ICollection<Pictures> Pictures { get; set; }
    }
}
