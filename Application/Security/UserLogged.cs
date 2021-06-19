using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Contracts;
using Business;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Security
{
    public class UserLogged
    {
        public class Execute : IRequest<UserData>
        {
            
        }

        public class Handler : IRequestHandler<Execute, UserData>
        {
            public readonly UserManager<User> _userManager;
            public readonly IJwtGenerator _jwtGenerator;
            public readonly IUserSession _userSession;
            public Handler(UserManager<User> userManager, IJwtGenerator jwtGenerator, IUserSession userSession)
            {
                _userManager = userManager;
                _jwtGenerator = jwtGenerator;
                _userSession = userSession;
            }
            public async Task<UserData> Handle(Execute request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByNameAsync(_userSession.GetUserSession());

                var rolesResult = await _userManager.GetRolesAsync(user);
                var rolesList = new List<string>(rolesResult);

                return new UserData
                {
                    FullName = user.FullName,
                    Username = user.UserName,
                    Email = user.Email,    
                    Token = _jwtGenerator.CreateToken(user, rolesList),
                    Image = null
                };
            }
        }
    }
}