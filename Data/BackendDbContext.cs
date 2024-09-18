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

        public DbSet<Event> Events { get; set; }

        public DbSet<EventAttendee> EventAttendees { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Picture> Pictures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<EventAttendee>()
                .HasKey(ea => new { ea.UserId, ea.EventId });

            modelBuilder
                .Entity<EventAttendee>()
                .HasOne(ea => ea.User)
                .WithMany(u => u.AttendingEvents)
                .HasForeignKey(ea => ea.UserId);

            modelBuilder
                .Entity<EventAttendee>()
                .HasOne(ea => ea.Event)
                .WithMany(e => e.Attendees)
                .HasForeignKey(ea => ea.EventId);
        }
    }
}
