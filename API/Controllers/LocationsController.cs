using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Controllers
{
    public class LocationsController : BaseApiController
    {
        private readonly DataContext _context;

        public LocationsController(DataContext context)
        {
            _context = context;
   
        }

        [HttpGet]
        public async Task<ActionResult<List<Location>>> GetLocations()
        {
            return await _context.Locations.ToListAsync();
        }

        [HttpGetAttribute("{id}")]
        public async Task<ActionResult<Location>> GetLocation(int id)
        {
            return await _context.Locations.FindAsync(id);
        }
    }
}