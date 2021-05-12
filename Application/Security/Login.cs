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

            public Handler(UserManager<User> userManager, SignInManager<User> signInManager)
            {
                _userManager = userManager;
                _signInManager = signInManager;
            }
            public async Task<UserData> Handle(Execute request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if(user == null)
                {
                    throw new ExceptionHandler(HttpStatusCode.Unauthorized);
                }

                var response = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

                if(response.Succeeded)
                {
                    return new UserData
                    {
                        FullName = user.FullName,
                        Token = "holis soy el token",
                        Username = user.UserName,
                        Email = user.Email,
                        Imagen = null
                    };
                }
                
                throw new ExceptionHandler(HttpStatusCode.Unauthorized);

            }
        }

    }
}