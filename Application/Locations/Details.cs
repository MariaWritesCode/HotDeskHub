using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Persistence;

namespace Application.Locations
{
    public class Details
    {
        public class Query : IRequest<Location>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Location>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;

            }

            public async Task<Location> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Locations.FindAsync(request.Id);
            }
        }
    }
}