using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class LocationRepository : ILocationRepository
    {
        private readonly DataContext _context;
        
        public LocationRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Location> FindLocationWithDesksById(int id)
        {
            return await _context.Locations.Include(location => location.Desks).FirstOrDefaultAsync(location => location.Id == id);
        }

        public void Remove(Location location)
        {
            _context.Locations.Remove(location);
        }

        public async Task<int> Save()
        {
            return await _context.SaveChangesAsync();
        }
    }
}