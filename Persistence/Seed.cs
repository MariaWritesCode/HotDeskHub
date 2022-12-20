using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace Persistence
{
    public class Seed
    {
        public static async Task SeedData(DataContext context)
        {
            await SeedLocations(context);
            await SeedEmployees(context);

            await context.SaveChangesAsync();
        }

        public static async Task SeedLocations(DataContext context)
        {
            if (context.Locations.Any()) return;

            var locations = new List<Location>
            {
                new Location
                {
                    Building = "Podium1",
                    Floor = 1,
                    Room = "32b",
                    Desks = new List<Desk>
                    {
                        new Desk
                        {
                            Available = true
                        },
                        new Desk
                        {
                            Available = false
                        }
                    }
                },
                new Location
                {
                    Building = "Podium1",
                    Floor = 3,
                    Room = "45f",
                    Desks = new List<Desk>
                    {
                        new Desk
                        {
                            Available = true
                        },
                        new Desk
                        {
                            Available = false
                        }
                    }
                },
                new Location
                {
                    Building = "Podium1",
                    Floor = 5,
                    Room = "51b",
                },
                new Location
                {
                    Building = "Podium2",
                    Floor = 2,
                    Room = "12a",
                },

               new Location
                {
                    Building = "Podium1",
                    Floor = 4,
                    Room = "19a",
                },
            };

            await context.Locations.AddRangeAsync(locations);
        }

        public static async Task SeedEmployees(DataContext context)
        {
            if (context.Employees.Any()) return;

            var employees = new List<Employee>
            {
                new Employee
                {
                    FirstName = "Bob",
                    LastName = "Builder",
                    IsAdministrator = false
                },
                new Employee
                {
                    FirstName = "Sam",
                    LastName = "Fireman",
                    IsAdministrator = true
                }
            };

            await context.Employees.AddRangeAsync(employees);
        }
    }
}