using Business;

namespace Application.Contracts
{
    public interface IJwtGenerator
    {
        string CreateToken(User user);
    }
}