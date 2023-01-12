using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using static Application.Account.Login;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login(Command command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}