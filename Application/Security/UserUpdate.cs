using System;
using System.Collections.Generic;
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
    public class UserUpdate
    {
        public class Execute : IRequest<UserData>
        {
            public string Name { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string UserName { get; set; }
        }

        public class ExecuteValidation : AbstractValidator<Execute>
        {
            public ExecuteValidation()
            {
                RuleFor(u=>u.Name).NotEmpty();
                RuleFor(u=>u.LastName).NotEmpty();
                RuleFor(u=>u.Email).NotEmpty();
                RuleFor(u=>u.Password).NotEmpty();
                RuleFor(u=>u.UserName).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Execute, UserData>
        {
            private readonly ElearningCatalogContext _context;
            private readonly UserManager<User> _userManager;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly IPasswordHasher<User> _passwordHasher;

            public Handler(ElearningCatalogContext context, UserManager<User> userManager, IJwtGenerator jwtGenerator, IPasswordHasher<User> passwordHasher)
            {
                _context = context;
                _userManager = userManager;
                _jwtGenerator = jwtGenerator;
                _passwordHasher = passwordHasher;
            }
            
            public async Task<UserData> Handle(Execute request, CancellationToken cancellationToken)
            {
                var userIden = await _userManager.FindByNameAsync(request.UserName);
                if(userIden == null)
                {
                    throw new ExceptionHandler(HttpStatusCode.NotFound, new {message = "User not found."});
                }

                var result = await _context.Users.Where(u => u.Email == request.Email && u.UserName != request.UserName).AnyAsync();
                if(result)
                {
                    throw new ExceptionHandler(HttpStatusCode.InternalServerError, new {message = "The email already exists."});
                }

                userIden.FullName = request.Name + " " + request.LastName;
                userIden.PasswordHash = _passwordHasher.HashPassword(userIden, request.Password);
                userIden.Email = request.Email;

                var updated = await _userManager.UpdateAsync(userIden);

                var rolesResult = await _userManager.GetRolesAsync(userIden);
                var roles = new List<string>(rolesResult);
                if(updated.Succeeded)
                {
                    return new UserData
                    {
                        FullName = userIden.FullName,
                        Username = userIden.UserName,
                        Email = userIden.Email,
                        Token = _jwtGenerator.CreateToken(userIden, roles)
                    };
                }

                throw new Exception("Cannot update user.");
            }
        }
    }
}