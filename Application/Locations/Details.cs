using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Desks;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Locations
{
    public class Details
    {
        public class Query : IRequest<LocationDto>
        {
            public int Id { get; set; }
            public bool IsAdmin { get; set; }
        }

        public class Handler : IRequestHandler<Query, LocationDto>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<LocationDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var location = request.IsAdmin ?
                    await _context.Locations
                        .Include(location => location.Desks)
                        .ThenInclude(location => location.Reservations)
                        .ThenInclude(reservation => reservation.Employee)
                        .SingleOrDefaultAsync(location => location.Id == request.Id) :
                    await _context.Locations
                        .Include(location => location.Desks)
                        .ThenInclude(location => location.Reservations)
                        .SingleOrDefaultAsync(location => location.Id == request.Id);

                return _mapper.Map<LocationDto>(location);
            }
        }
    }
}