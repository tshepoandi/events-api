using Microsoft.EntityFrameworkCore;
using backends.Entities;

namespace backends.Data
{
    public class BackendsDbContext : DbContext
    {
        public BackendsDbContext(DbContextOptions<BackendsDbContext> options) :
            base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
