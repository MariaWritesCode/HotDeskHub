using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Locations
{
    public class List
    {
        public class Query: IRequest<List<Location>> {}

        public class Handler : IRequestHandler<Query, List<Location>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }
            
            public async Task<List<Location>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Locations.ToListAsync();
            }
        }
    }
}