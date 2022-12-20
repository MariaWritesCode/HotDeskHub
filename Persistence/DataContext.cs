using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Location> Locations { get; set; }
        public DbSet<Desk> Desks { get; set; }
        public DbSet<Employee> Employees { get; set; }
    }
}