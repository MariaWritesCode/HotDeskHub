using Xunit;
using NSubstitute;
using System.Formats.Asn1;
using Domain;
using Persistence;
using Application.UnitTests.TestUtils;
using Microsoft.EntityFrameworkCore;
using static Application.Desks.Delete;

namespace Application.UnitTests.Desks
{
    public class DeleteTests
    {
        private readonly DataContext _dataContext;

        private readonly Handler _sut;
        public DeleteTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase("fakeDb")
                .Options;

            _dataContext = Substitute.For<DataContext>(options);

            _dataContext.Reservations = new List<Reservation>().FakeDbSet();

            _sut = new(_dataContext);
        }


        [Fact]
        public async Task NullDeskId_ShouldThrow()
        {
           //Arrange
           var desk = new Desk()
           {
               Id = 1,
               Reservations = new List<Reservation>() { new Reservation { DeskId = 1, Date = DateTime.Now.AddDays(1) } },
               Available = true
           };

            var desks = new List<Desk>() { desk }.FakeDbSet();
            _dataContext.Desks = desks;

            Command testCommand = new() { DeskId = 2 };

            // Act & Assert

            var exception = await Assert.ThrowsAsync<NullReferenceException>(() => _sut.Handle(testCommand, default));

            Assert.Equal("You can't delete this desk because it doesn't exists", exception.Message);
            await _dataContext.DidNotReceive().SaveChangesAsync();


        }

        [Fact]
        public async Task ReservationExist_ShouldThrow()
        {
            //Arrange
            var desk = new Desk()
            {
                Id = 1,
                Reservations = new List<Reservation>() { new Reservation { DeskId = 1, Date = DateTime.Now.AddDays(1) } },
                Available = true
            };

            var desks = new List<Desk>() { desk }.FakeDbSet();
            _dataContext.Desks = desks;

            Command testCommand = new() { DeskId = 1 };

            // Act & Assert

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.Handle(testCommand, default));

            Assert.Equal("You can't remove desk because it contains some revervations.", exception.Message);
            await _dataContext.DidNotReceive().SaveChangesAsync();


        }

        [Fact]
        public async Task DeskMatched_ShouldRemoveDesk()
        {
            //Arrange
            var desk = new Desk()
            {
                Id = 1,
                Reservations = new List<Reservation>(),
                Available = true
            };

            var desks = new List<Desk>() { desk }.FakeDbSet();
            _dataContext.Desks = desks;

            Command testCommand = new() { DeskId = 1 };

            // Act & Assert
            await _sut.Handle(testCommand, default);

            await _dataContext.Received(1).SaveChangesAsync();

            _dataContext.Desks.Received(1).Remove(Arg.Is<Desk>(desk => desk.Id == 1));
        }

    }
}
