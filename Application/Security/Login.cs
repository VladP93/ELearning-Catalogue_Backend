using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Contracts;
using Application.ErrorHandler;
using Business;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Security
{
    public class Login
    {
        public class Execute : IRequest<UserData>
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class ValidationExecute : AbstractValidator<Execute>
        {
            public ValidationExecute()
            {
                RuleFor(r => r.Email ).NotEmpty();
                RuleFor(r => r.Password).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Execute, UserData>
        {
            private readonly UserManager<User> _userManager;
            private readonly SignInManager<User> _signInManager;
            private readonly IJwtGenerator _jwtGenerator;

            public Handler(UserManager<User> userManager, SignInManager<User> signInManager, IJwtGenerator jwtGenerator)
            {
                _userManager = userManager;
                _signInManager = signInManager;
                _jwtGenerator = jwtGenerator;
            }

            public async Task<UserData> Handle(Execute request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if(user == null)
                {
                    throw new ExceptionHandler(HttpStatusCode.Unauthorized);
                }

                var response = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

                var rolesResult = await _userManager.GetRolesAsync(user);
                var rolesList = new List<string>(rolesResult);

                if(response.Succeeded)
                {
                    return new UserData
                    {
                        FullName = user.FullName,
                        Token = _jwtGenerator.CreateToken(user, rolesList),
                        Username = user.UserName,
                        Email = user.Email,
                        Image = null
                    };
                }
                
                throw new ExceptionHandler(HttpStatusCode.Unauthorized);

            }
        }

    }
}