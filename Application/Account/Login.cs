using System;
using Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Persistence;
using Microsoft.AspNetCore.Identity;

namespace Application.Account
{
    public class Login
    {
        public class Command : IRequest
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly SignInManager<Employee> _signInManager;

            public Handler(SignInManager<Employee> signInManager)
            {
                _signInManager = signInManager;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var result = await _signInManager.PasswordSignInAsync(request.UserName, request.Password, false, false);

                return Unit.Value;
            }
        }
    }
}