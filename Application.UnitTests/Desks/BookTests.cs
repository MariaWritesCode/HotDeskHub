using Xunit;
using NSubstitute;
using System.Formats.Asn1;
using Domain;
using Persistence;
using Application.UnitTests.TestUtils;
using Microsoft.EntityFrameworkCore;
using static Application.Desks.Book;
namespace Application.UnitTests.Desks
{
    public class BookTests
    {
        private readonly Employee? _employee;
        private readonly Desk _desk;

        private readonly DataContext _dataContext;
        private readonly DbSet<Employee> _employees;

        private readonly Handler _sut;
        public BookTests()
        {
            _employee = new() { Id = 1, FirstName = "Fake", LastName = "Employee" };
            _desk = new();

            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase("fakeDb")
                .Options;

            _dataContext = Substitute.For<DataContext>(options);
            _employees = new List<Employee>() { _employee }.FakeDbSet();

            _dataContext.Employees = _employees;
            _dataContext.Employees.FindAsync(Arg.Any<object>()).Returns(ValueTask.FromResult(_employee));
            _dataContext.Reservations = new List<Reservation>().FakeDbSet();

            _sut = new(_dataContext);
        }

        [Fact]
        public async Task Handle_NullEmployeeId_ShouldThrow()
        {
            // Arrange

            // Act

            // Assert
        }

        [Fact]
        public async Task DeskIsNull_ShouldThrow()
        {
            // Arrange

            // Act

            // Assert
        }

        [Fact]
        public async Task DeskHasReservations_ShouldThrow()
        {
            // Arrange

            // Act

            // Assert
        }

        [Fact]
        public async Task DateEarlierThanToday_ShouldThrow()
        {
            // Arrange

            // Act

            // Assert
        }

        [Fact]
        public async Task ReservationValid_ShouldAddToDesk_SaveToContext()
        {
            // Arrange
            var desk = new Desk()
            {
                Id = 1,
                Reservations = new List<Reservation>() { },
                Available = true
            };

            var desks = new List<Desk>() { desk }.FakeDbSet();
            _dataContext.Desks = desks;

            Command testCommand = new() { Date = DateTime.Now.AddDays(1), DeskId = 1, EmployeeId = 1 };

            // Act
            await _sut.Handle(testCommand, default);

            // Assert
            await _dataContext.Received(1).SaveChangesAsync();
            Assert.NotEmpty(desk.Reservations);
        }
    }
}
