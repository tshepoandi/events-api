using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backends.Entities
{
    [Table("pictures")]
    public class Picture
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Url { get; set; }

        [ForeignKey("ReviewId")]
        public Review Review { get; set; }

        public int ReviewId { get; set; }
    }
}
