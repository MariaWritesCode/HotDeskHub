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
                var desk = await _context.Desks.FindAsync(request.DeskId);

                desk.Available = !desk.Available;

                _context.Desks.Remove(desk);

                await _context.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}