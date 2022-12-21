using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Desks
{
    public class Delete
    {
        public class Command : IRequest
        {
            public int DeskId { get; set; }
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
                var desk = await _context.Desks
                    .Include(desk => desk.Reservations)
                    .SingleOrDefaultAsync(desk => desk.Id == request.DeskId);

                if(desk == null)
                    throw new NullReferenceException("You can't delete this desk because it doesn't exists");

                if(desk.Reservations.Any())
                    throw new InvalidOperationException("You can't remove desk because it contains some revervations.");

                _context.Desks.Remove(desk);

                await _context.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}