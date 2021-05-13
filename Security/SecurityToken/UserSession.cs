using System.Linq;
using System.Security.Claims;
using Application.Contracts;
using Microsoft.AspNetCore.Http;

namespace SecurityToken
{
    public class UserSession : IUserSession
    {
        private readonly IHttpContextAccessor _httpContextAccesor;

        public UserSession(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccesor = httpContextAccessor;
        }

        public string GetUserSession()
        {
            var userName = _httpContextAccesor.HttpContext.User?.Claims?.FirstOrDefault(u => u.Type==ClaimTypes.NameIdentifier)?.Value;
            return userName;
        }
    }
}