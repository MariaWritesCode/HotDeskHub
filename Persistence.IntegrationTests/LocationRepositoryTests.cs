using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using NUnit.Framework;

namespace Persistence.IntegrationTests
{
    public class LocationRepositoryTests : SqliteTests
    {
        protected override void SeedDatabase(DataContext context)
        {
            var locations = new[]
            {
                new Location { Id = 1, Desks = { new Desk() }},
            };

            context.Locations.AddRange(locations);
        }

        [Test]
        public async Task FindLocationWithDesksById_IdOfExistingLocation_ReturnsLocation()
        {
            using var context = CreateContext();
            var locationRepository = new LocationRepository(context);
            var locationId = 1;

            var location = await locationRepository.FindLocationWithDesksById(locationId);

            Assert.AreEqual(locationId, location.Id);
        }

        [Test]
        public async Task FindLocationWithDesksById_IdOfNonExistingLocation_ReturnsNull()
        {
            using var context = CreateContext();
            var locationRepository = new LocationRepository(context);
            var locationId = 2;

            var location = await locationRepository.FindLocationWithDesksById(locationId);

            Assert.IsNull(location);
        }

        [Test]
        public async Task FindLocationWithDesksById_LocationFound_IncludesDesks()
        {
            using var context = CreateContext();
            var locationRepository = new LocationRepository(context);
            var locationId = 1;

            var location = await locationRepository.FindLocationWithDesksById(locationId);

            Assert.AreEqual(1, location.Desks.Count);
        }

        [Test]
        public async Task Remove_LocationToBeRemoved_LocationRemovedFromLocations()
        {
            using var context = CreateContext();
            var locationRepository = new LocationRepository(context);
            var locationId = 1;

            var location = await locationRepository.FindLocationWithDesksById(locationId);

            locationRepository.Remove(location);
            await locationRepository.Save();

            Assert.AreEqual(0, context.Locations.Count());
        }
    }
}