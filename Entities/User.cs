using System.ComponentModel.DataAnnotations.Schema;
namespace backends.Entities
{
    [Table("user")]
    public class User
    {
        [Column("id")]
        public Guid Id { get; set; }
        [Column("name")]
        public required string Name { get; set; }
        [Column("email")]
        public required string Email { get; set; }
    }
}
