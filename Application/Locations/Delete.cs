using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Locations
{
    public class Delete
    {
        public class Command : IRequest
        {
            public int Id { get; set; }
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
                var location = await _context.Locations.Include(location => location.Desks).FirstOrDefaultAsync(location => location.Id == request.Id);

                if(location == null)
                    throw new NullReferenceException("You can't delete this location because it doesn't exists");

                if(location.Desks.Any())
                    throw new InvalidOperationException("You can't remove location because it contains some desks. Please remove all desks from location first.");

                _context.Locations.Remove(location);

                await _context.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}