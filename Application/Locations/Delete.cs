using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using MediatR;

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
            private readonly ILocationRepository _locationRepository;
            public Handler(ILocationRepository locationRepository)
            {
                _locationRepository = locationRepository;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var location = await _locationRepository.FindLocationWithDesksById(request.Id);

                if (location == null)
                    throw new NullReferenceException("You can't delete this location because it doesn't exists");

                if (location.Desks.Any())
                    throw new InvalidOperationException("You can't remove location because it contains some desks. Please remove all desks from location first.");

                _locationRepository.Remove(location);

                var result = await _locationRepository.Save();

                return Unit.Value;
            }
        }
    }
}