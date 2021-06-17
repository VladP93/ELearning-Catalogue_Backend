using System;
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
    public class DeleteUserRole
    {
        public class Execute : IRequest
        {
            public string UserName { get; set; }
            public string RoleName { get; set; }
        }

        public class ExecuteValidation : AbstractValidator<Execute>
        {
            public ExecuteValidation()
            {
                RuleFor(d=>d.UserName).NotEmpty();
                RuleFor(d=>d.RoleName).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Execute>
        {
            private readonly UserManager<User> _userManager;
            private readonly RoleManager<IdentityRole> _roleManager;

            public Handler(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
            {
                _userManager = userManager;
                _roleManager = roleManager;
            }

            public async Task<Unit> Handle(Execute request, CancellationToken cancellationToken)
            {
                var role = await _roleManager.FindByNameAsync(request.RoleName);
                if(role == null)
                {
                    throw new ExceptionHandler(HttpStatusCode.BadRequest, new {message = "Role does not exists."});
                }

                var userIden = await _userManager.FindByNameAsync(request.UserName);
                if(userIden == null)
                {
                    throw new ExceptionHandler(HttpStatusCode.BadRequest, new {message = "User does not exists."});
                }

                var result = await _userManager.RemoveFromRoleAsync(userIden, request.RoleName);

                if(result.Succeeded)
                {
                    return Unit.Value;
                }

                throw new Exception("Cannot remove role from user.");
            }
        }
    }
}