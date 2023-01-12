using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Desks;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Controllers
{
    [Authorize]
    public class DesksController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<List<DeskDto>>> GetDesks([FromQuery] int locationId)
        {
            return await Mediator.Send(new List.Query { LocationId = locationId });
        }

        [HttpGetAttribute("{id}")]
        public async Task<ActionResult<DeskDto>> GetDesk(int id)
        {
            var isAdmin = User.FindFirst(ClaimTypes.Role)?.Value == "Admin";
            return await Mediator.Send(new Details.Query { Id = id,  IsAdmin = isAdmin  });
        }

        [HttpPost("{deskId}/book")]
        public async Task<IActionResult> BookDesk([FromRoute]int deskId, [FromBody]DateTime date)
        {
            var command = new Book.Command
            {
                EmployeeId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value),
                DeskId = deskId,
                Date = date,
            };

            return Ok(await Mediator.Send(command));
        }

        [HttpPut("{deskId}/book")]
        public async Task<IActionResult> ChangeDesk(int deskId, Rebook.Command command)
        {
            command.FromDeskId = deskId;
            return Ok(await Mediator.Send(command));
        }
    }
}