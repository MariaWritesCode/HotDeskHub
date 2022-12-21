using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Desks
{
    public class Book
    {
        public class Command : IRequest
        {
            public int DeskId { get; set; }
            public int EmployeeId { get; set; }
            public DateTime Date { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var employee = await _context.Employees.FindAsync(request.EmployeeId);

                if (employee == null)
                    throw new InvalidOperationException("You can't book this desk because specified employee doesn't exists");

                var desk = await _context.Desks
                    .Include(desk => desk.Reservations)
                    .ThenInclude(reservation => reservation.Employee)
                    .SingleOrDefaultAsync(desk => desk.Id == request.DeskId);

                if (desk == null)
                    throw new InvalidOperationException("You can't book this desk because it doesn't exists");

                if (!desk.Available)
                    throw new InvalidOperationException("You can't book this desk because it is unavailable");

                if (desk.Reservations.Any(reservation => reservation.Date == request.Date.Date))
                    throw new InvalidOperationException("You can't book this desk because it is already booked");

                if (desk.Reservations.Count(reservation => reservation.Employee == employee) >= 7)
                    throw new InvalidOperationException("You can't book this desk because you already reserved it for 7 days");

                if (request.Date < DateTime.Now.Date)
                    throw new InvalidOperationException("You can't book desk before today");

                var reservation = new Reservation
                {
                    Employee = employee,
                    Desk = desk,
                    Date = request.Date.Date,
                };

                desk.Reservations.Add(reservation);

                await _context.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}