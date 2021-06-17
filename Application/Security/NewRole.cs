using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.ErrorHandler;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Security
{
    public class NewRole
    {
        public class Execute : IRequest
        {
            public string Name { get; set; }
        }

        public class ExecuteValidation : AbstractValidator<Execute>
        {
            public ExecuteValidation()
            {
                RuleFor(r=>r.Name).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Execute>
        {
            private readonly RoleManager<IdentityRole> _roleManager;
            public Handler(RoleManager<IdentityRole> roleManager)
            {
                _roleManager = roleManager;
            }
            public async Task<Unit> Handle(Execute request, CancellationToken cancellationToken)
            {
                var role = await _roleManager.FindByNameAsync(request.Name);
                if(role!=null)
                {
                    throw new ExceptionHandler(HttpStatusCode.BadRequest, new {message = "Role already exists."});
                }

                var result = await _roleManager.CreateAsync(new IdentityRole(request.Name));

                if(result.Succeeded)
                {
                    return Unit.Value;
                }

                throw new Exception("Cannot create role.");
            }
        }
    }
}