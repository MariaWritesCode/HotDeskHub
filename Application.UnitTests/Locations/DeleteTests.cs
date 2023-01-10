using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using NSubstitute;
using NUnit.Framework;
using static Application.Locations.Delete;

namespace Application.UnitTests.Locations
{
    public class DeleteTests
    {
        private ILocationRepository _fakeLocationRepository;
        private Handler _handler;

        [SetUp]
        public void SetUp()
        {
            _fakeLocationRepository = Substitute.For<ILocationRepository>();
            _handler = new Handler(_fakeLocationRepository);
        }

        [Test]
        public void Delete_LocationIsNull_ThrowsNullReferenceException()
        {
            var exception = Assert.ThrowsAsync<NullReferenceException>(() => _handler.Handle(new Command(), default));

            Assert.AreEqual("You can't delete this location because it doesn't exists", exception?.Message);
        }

        [Test]
        public void Delete_LocationDesksNotEmpty_ThrowsInvalidOperationException()
        {
            int locationId = 1;
            _fakeLocationRepository.FindLocationWithDesksById(locationId).Returns(new Location { Desks = new List<Desk> { new Desk() } });

            var exception = Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(new Command { Id = locationId }, default));

            Assert.AreEqual("You can't remove location because it contains some desks. Please remove all desks from location first.", exception?.Message);
        }

        [Test]
        public async Task Delete_LocationDesksAreEmpty_LocationRemoved()
        {
            int locationId = 1;
            var locationWithoutDesks = new Location();
            _fakeLocationRepository.FindLocationWithDesksById(locationId).Returns(locationWithoutDesks);

            await _handler.Handle(new Command { Id = locationId }, default);

            _fakeLocationRepository.Received(1).Remove(locationWithoutDesks);
            await _fakeLocationRepository.Received(1).Save();
        }
    }
}