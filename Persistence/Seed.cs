using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Identity;

namespace Persistence
{
    public class Seed
    {
        public static async Task SeedData(DataContext context, UserManager<Employee> userManager)
        {
            await SeedLocations(context);
            await SeedEmployees(userManager);

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

        public static async Task SeedEmployees(UserManager<Employee> userManager)
        {
            if (userManager.Users.Any()) return;

            var bob = new Employee
            {
                FirstName = "Bob",
                LastName = "Builder",
                UserName = "bob@test.com",
            };

            var sam = new Employee
            {
                FirstName = "Sam",
                LastName = "Fireman",
                UserName = "sam@test.com",
            };


            await userManager.CreateAsync(bob, "Developer123");
            await userManager.AddClaimAsync(bob, new Claim(ClaimTypes.Role, "User"));
            await userManager.CreateAsync(sam, "Developer123");
            await userManager.AddClaimAsync(sam, new Claim(ClaimTypes.Role, "Admin"));
        }
    }
}