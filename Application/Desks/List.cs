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
    public class List
    {
        public class Query : IRequest<List<DeskDto>>
        {
            public int Location { get; set; }
        }

        public class Handler : IRequestHandler<Query, List<DeskDto>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<List<DeskDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var desks = await _context.Desks
                    .Where(desk => request.Location == default || desk.Location.Id == request.Location)
                    .ToListAsync();

                return _mapper.Map<List<DeskDto>>(desks);
            }
        }
    }
}