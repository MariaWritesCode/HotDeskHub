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
            if (context.Locations.Any()) return;

            var locations = new List<Location>
            {
                new Location
                {
                    Building = "Podium1",
                    Floor = 1,
                    Room = "32b",
                },
                new Location
                {
                    Building = "Podium1",
                    Floor = 3,
                    Room = "45f",
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
            await context.SaveChangesAsync();
        }

    }
}