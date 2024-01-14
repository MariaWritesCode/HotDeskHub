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

        private readonly DataContext _dataContext;
        private readonly DbSet<Employee> _employees;

        private readonly Handler _sut;
        public BookTests()
        {
            _employee = new() { Id = 1, FirstName = "Fake", LastName = "Employee" };

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
            _dataContext.Employees.FindAsync(Arg.Any<object>()).Returns(ValueTask.FromResult<Employee>(default));
            Command testCommand = new() { Date = DateTime.Now.AddDays(1), DeskId = 1, EmployeeId = 1 };
            // Act & Assert

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.Handle(testCommand, default));
            Assert.Equal("You can't book this desk because specified employee doesn't exists", exception.Message);
            await _dataContext.DidNotReceive().SaveChangesAsync();


        }

        [Fact]
        public async Task DeskIsNull_ShouldThrow()
        {
            // Arrange
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

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.Handle(testCommand, default));
            Assert.Equal("You can't book this desk because it doesn't exists", exception.Message);
            //var exception = await Record.ExceptionAsync(() => _sut.Handle(testCommand, default));
            //Assert.NotNull(exception);
            //Assert.IsType<InvalidOperationException>(exception);

        }

        [Fact]
        public async Task DeskHasReservations_ShouldThrow()
        {
            // Arrange
            var desk = new Desk()
            {
                Id = 1,
                Reservations = new List<Reservation>() { new Reservation { DeskId = 1, Date = DateTime.Now.AddDays(1) } },
                Available = true
            };


            var desks = new List<Desk>() { desk }.FakeDbSet();
            _dataContext.Desks = desks;

            Command testCommand = new() { Date = DateTime.Now.AddDays(1), DeskId = 1, EmployeeId = 1 };

            // Act & Assert

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.Handle(testCommand, default));
            Assert.Equal("You can't book this desk because it is already booked", exception.Message);
            //var exception = await Record.ExceptionAsync(() => _sut.Handle(testCommand, default));
            //Assert.NotNull(exception);
            //Assert.IsType<InvalidOperationException>(exception);
        }

        [Fact]
        public async Task DateEarlierThanToday_ShouldThrow()
        {
            // Arrange

            var desk = new Desk()
            {
                Id = 1,
                Reservations = new List<Reservation>() { new Reservation { DeskId = 1, Date = DateTime.Now.AddDays(1) } },
                Available = true
            };

            var desks = new List<Desk>() { desk }.FakeDbSet();
            _dataContext.Desks = desks;

            Command testCommand = new() { Date = DateTime.Now.AddDays(-1), DeskId = 1, EmployeeId = 1 };
            // Act & Assert

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.Handle(testCommand, default));
            Assert.Equal("You can't book desk before today", exception.Message);
            //var exception = await Record.ExceptionAsync(() => _sut.Handle(testCommand, default));
            //Assert.NotNull(exception);
            //Assert.IsType<InvalidOperationException>(exception);
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
