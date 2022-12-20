using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Persistence;

namespace Application.Desks
{
    public class Details
    {
        public class Query : IRequest<Desk>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Desk>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;

            }

            public async Task<Desk> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Desks.FindAsync(request.Id);
            }
        }
    }
}