using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DataAccess;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Security
{
    public class RolesQuery
    {
        public class Execute : IRequest<List<IdentityRole>>
        {

        }

        public class Hanlder : IRequestHandler<Execute, List<IdentityRole>>
        {
            private readonly ElearningCatalogContext _context;

            public Hanlder(ElearningCatalogContext context)
            {
                _context = context;
            }
            public async Task<List<IdentityRole>> Handle(Execute request, CancellationToken cancellationToken)
            {
                var roles = await _context.Roles.ToListAsync();
                
                return roles;
            }
        }
    }
}