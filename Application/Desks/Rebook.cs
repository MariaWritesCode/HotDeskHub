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
    public class Rebook
    {
        public class Command : IRequest
        {
            public int ToDeskId { get; set; }
            public int FromDeskId { get; set; }
            public DateTime Date { get; set; }
            public int EmployeeId { get; set; }
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
                var hoursTillReservation = (request.Date.Date - DateTime.Now.Date).TotalHours;

                if (hoursTillReservation < 24)
                    throw new InvalidOperationException("You can't modify reservation later than 24h before reservation");

                var currentReservation = await _context.Reservations.FindAsync(request.EmployeeId, request.FromDeskId, request.Date);

                if (currentReservation == null)
                    throw new InvalidOperationException("You can't modify reservation because it doesn't exist");

                var isTargetDeskAlreadyTaken = await _context.Reservations.AnyAsync(reservation => reservation.DeskId == request.ToDeskId && reservation.Date == request.Date.Date);

                if (isTargetDeskAlreadyTaken)
                    throw new InvalidOperationException("Can't change to given desk because it is already reserved");

                var targetDesk = await _context.Desks.FindAsync(request.ToDeskId);

                if (targetDesk == null)
                    throw new InvalidOperationException("Can't change to given desk because it doesn't exist");

                _context.Reservations.Remove(currentReservation);
                _context.Reservations.Add(new Reservation { EmployeeId = request.EmployeeId, DeskId = request.ToDeskId, Date = request.Date.Date });

                await _context.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}