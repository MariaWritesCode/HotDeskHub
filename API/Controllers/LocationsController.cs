using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Desks;
using Application.Locations;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Controllers
{
    [Authorize]
    public class LocationsController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<List<LocationDto>>> GetLocations()
        {
            return await Mediator.Send(new Application.Locations.List.Query());
        }

        [HttpGetAttribute("{id}")]
        public async Task<ActionResult<LocationDto>> GetLocation(int id)
        {
            var isAdmin = User.FindFirst(ClaimTypes.Role)?.Value == "Admin";

            return await Mediator.Send(new Application.Locations.Details.Query { Id = id, IsAdmin = isAdmin });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateLocation(Location location)
        {
            return Ok(await Mediator.Send(new Application.Locations.Create.Command { Location = location }));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditLocation(int id, Location location)
        {
            location.Id = id;
            return Ok(await Mediator.Send(new Application.Locations.Edit.Command { Location = location }));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            return Ok(await Mediator.Send(new Application.Locations.Delete.Command { Id = id }));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("{locationId}/desks")]
        public async Task<IActionResult> CreateDesk(int locationId, Desk desk)
        {
            return Ok(await Mediator.Send(new Application.Desks.Create.Command { LocationId = locationId, Desk = desk }));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{locationId}/desks/{deskId}")]
        public async Task<IActionResult> RemoveDesk(int deskId)
        {
            return Ok(await Mediator.Send(new Application.Desks.Delete.Command { DeskId = deskId }));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{locationId}/desks/{deskId}")]
        public async Task<IActionResult> ToggleAvailability(int locationId, int deskId)
        {
            return Ok(await Mediator.Send(new Application.Desks.Edit.Command { DeskId = deskId }));
        }
    }
}