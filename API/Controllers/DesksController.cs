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
        public async Task<ActionResult<List<Desk>>> GetDesks([FromQuery] int location)
        {
            return await Mediator.Send(new List.Query { Location = location });
        }

        [HttpGetAttribute("{id}")]
        public async Task<ActionResult<Desk>> GetDesk(int id)
        {
            return await Mediator.Send(new Details.Query { Id = id });
        }
    }
}