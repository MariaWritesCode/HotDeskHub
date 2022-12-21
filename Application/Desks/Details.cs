using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Desks
{
    public class Details
    {
        public class Query : IRequest<DeskDto>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, DeskDto>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            
            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<DeskDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var desk = await _context.Desks
                    .Include(desk => desk.Reservations)
                    .ThenInclude(reservation => reservation.Employee)
                    .SingleOrDefaultAsync(desk => desk.Id == request.Id);

                return _mapper.Map<DeskDto>(desk);
            }
        }
    }
}