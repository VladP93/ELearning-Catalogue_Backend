using System.Collections.Generic;
using Business;

namespace Application.Contracts
{
    public interface IJwtGenerator
    {
        string CreateToken(User user, List<string> roles);
    }
}