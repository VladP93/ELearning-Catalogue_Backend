using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Contracts;
using Application.ErrorHandler;
using Business;
using DataAccess;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Security
{
    public class UserRegister
    {
        public class Execute : IRequest<UserData>
        {
            public string FullName { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class ExecuteValidation : AbstractValidator<Execute>
        {
            public ExecuteValidation()
            {
                RuleFor(u => u.FullName).NotEmpty();
                RuleFor(u => u.UserName).NotEmpty();
                RuleFor(u => u.Email).NotEmpty();
                RuleFor(u => u.Password).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Execute, UserData>
        {
            private readonly ElearningCatalogContext _context;
            private readonly UserManager<User> _userManager;
            private readonly IJwtGenerator _jwtGenerator;

            public Handler(ElearningCatalogContext context, UserManager<User> userManager, IJwtGenerator jwtGenerator)
            {
                _context = context;
                _userManager = userManager;
                _jwtGenerator = jwtGenerator;
            }

            public async Task<UserData> Handle(Execute request, CancellationToken cancellationToken)
            {
                var existsEmail = await _context.Users.Where(u => u.Email == request.Email).AnyAsync();

                if (existsEmail)
                {
                    throw new ExceptionHandler(HttpStatusCode.BadRequest, new { message = "Email already registered" });
                }

                var existsUsername = await _context.Users.Where(u => u.UserName == request.UserName).AnyAsync();

                if (existsUsername)
                {
                    throw new ExceptionHandler(HttpStatusCode.BadRequest, new { message = "Username already registered" });
                }

                var user = new User
                {
                    FullName = request.FullName,
                    Email = request.Email,
                    UserName = request.UserName
                };

                var response = await _userManager.CreateAsync(user, request.Password);

                if (!response.Succeeded)
                {
                    throw new Exception("Cannot register new user.");
                }

                return new UserData
                {
                    FullName = user.FullName,
                    Email = user.Email,
                    Token = _jwtGenerator.CreateToken(user, null),
                    Username = user.UserName
                };

            }
        }
    }
}