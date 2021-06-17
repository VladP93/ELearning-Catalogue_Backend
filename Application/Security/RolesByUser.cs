using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.ErrorHandler;
using Business;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Security
{
    public class RolesByUser
    {
        public class Execute : IRequest<List<string>>
        {
            public string UserName { get; set; }
        }

        public class ExecuteValidation : AbstractValidator<Execute>
        {
            public ExecuteValidation()
            {
                RuleFor(r=>r.UserName).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Execute, List<string>>
        {
            private readonly UserManager<User> _userManager;
            private readonly RoleManager<IdentityRole> _roleManager;

            public Handler(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
            {
                _userManager = userManager;
                _roleManager = roleManager;
            }

            public async Task<List<string>> Handle(Execute request, CancellationToken cancellationToken)
            {
                var userIden = await _userManager.FindByNameAsync(request.UserName);
                if(userIden == null)
                {
                    throw new ExceptionHandler(HttpStatusCode.BadRequest, new {message = "User does not exists."});
                }

                var result = await _userManager.GetRolesAsync(userIden);

                return new List<string>(result);
            }
        }
    }
}