using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class DataContext : IdentityDbContext<Employee, IdentityRole<int>, int>
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Location> Locations { get; set; }
        public DbSet<Desk> Desks { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Reservation>(x => x.HasKey(reservation => new { reservation.EmployeeId, reservation.DeskId, reservation.Date }));

            builder.Entity<Reservation>()
                .HasOne(reservation => reservation.Employee)
                .WithMany(reservation => reservation.Reservations)
                .HasForeignKey(reservation => reservation.EmployeeId);

            builder.Entity<Reservation>()
                .HasOne(reservation => reservation.Desk)
                .WithMany(reservation => reservation.Reservations)
                .HasForeignKey(reservation => reservation.DeskId);
        }
    }
}