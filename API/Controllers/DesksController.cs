using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Desks;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Controllers
{
    public class DesksController : BaseApiController
    {

        [HttpGet]
        public async Task<ActionResult<List<DeskDto>>> GetDesks([FromQuery] int location)
        {
            return await Mediator.Send(new List.Query { Location = location });
        }

        [HttpGetAttribute("{id}")]
        public async Task<ActionResult<DeskDto>> GetDesk(int id)
        {
            return await Mediator.Send(new Details.Query { Id = id });
        }

        [HttpPost("{deskId}/book")]
        public async Task<IActionResult> BookDesk(int deskId, Book.Command command)
        {
            command.DeskId = deskId;
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