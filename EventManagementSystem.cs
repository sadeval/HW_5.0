using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;

namespace EventManagementSystem
{

    public class Event
    {
        public int EventId { get; set; }
        public string? Name { get; set; }
        public DateTime Date { get; set; }
        public virtual ICollection<EventGuest> EventGuests { get; set; } = new List<EventGuest>();
    }

    public class Guest
    {
        public int GuestId { get; set; }
        public string? Name { get; set; }
        public virtual ICollection<EventGuest> EventGuests { get; set; } = new List<EventGuest>();
    }

    public class EventGuest
    {
        public int EventId { get; set; }
        public virtual Event Event { get; set; }

        public int GuestId { get; set; }
        public virtual Guest Guest { get; set; }

        public string? Role { get; set; }
    }

    public class ApplicationContext : DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<EventGuest> EventGuests { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EventManagementSystem;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventGuest>()
                .HasKey(eg => new { eg.EventId, eg.GuestId });

            modelBuilder.Entity<EventGuest>()
                .HasOne(eg => eg.Event)
                .WithMany(e => e.EventGuests)
                .HasForeignKey(eg => eg.EventId);

            modelBuilder.Entity<EventGuest>()
                .HasOne(eg => eg.Guest)
                .WithMany(g => g.EventGuests)
                .HasForeignKey(eg => eg.GuestId);
        }
    }
}
