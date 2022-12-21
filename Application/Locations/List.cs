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
    public class List
    {
        public class Query : IRequest<List<LocationDto>> { }

        public class Handler : IRequestHandler<Query, List<LocationDto>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<List<LocationDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var locations = await _context.Locations.ToListAsync();

                return _mapper.Map<List<LocationDto>>(locations);
            }
        }
    }
}