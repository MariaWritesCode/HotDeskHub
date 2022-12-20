using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Locations;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Controllers
{
    public class LocationsController : BaseApiController
    {

        [HttpGet]
        public async Task<ActionResult<List<Location>>> GetLocations()
        {
            return await Mediator.Send(new List.Query());
        }

        [HttpGetAttribute("{id}")]
        public async Task<ActionResult<Location>> GetLocation(int id)
        {
            return await Mediator.Send(new Details.Query { Id = id });
        }

        [HttpPost]
        public async Task<IActionResult> CreateLocation(Location location)
        {
            return Ok(await Mediator.Send(new Create.Command { Location = location }));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditLocation(int id, Location location)
        {
            location.Id = id;
            return Ok(await Mediator.Send(new Edit.Command { Location = location }));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoation(int id)
        {
            return Ok(await Mediator.Send(new Delete.Command { Id = id }));
        }

        [HttpPost("{locationId}/desks")]
        public async Task<IActionResult> CreateDesk(int locationId, Desk desk)
        {
            return Ok(await Mediator.Send(new Application.Desks.Create.Command { LocationId = locationId, Desk = desk }));
        }

        [HttpDelete("{locationId}/desks/{deskId}")]
        public async Task<IActionResult> RemoveDesk(int deskId)
        {
            return Ok(await Mediator.Send(new Application.Desks.Delete.Command { DeskId = deskId }));
        }

        [HttpPut("{locationId}/desks/{deskId}")]
        public async Task<IActionResult> ToggleAvailability(int locationId, int deskId)
        {
            return Ok(await Mediator.Send(new Application.Desks.Edit.Command { DeskId = deskId  }));
        }
    }
}