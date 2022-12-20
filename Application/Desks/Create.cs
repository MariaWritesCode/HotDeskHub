using System;
using Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Persistence;

namespace Application.Desks
{
    public class Create
    {
        public class Command : IRequest
        {
            public int LocationId { get; set; }
            public Desk Desk { get; set; }
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
                var desk = request.Desk;
                
                var location = await _context.Locations.FindAsync(request.LocationId);
                desk.Location = location;

                _context.Desks.Add(desk);

                await _context.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}