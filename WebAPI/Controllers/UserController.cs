using System.Threading.Tasks;
using Application.Security;
using Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [AllowAnonymous]
    public class UserController : MyBaseController
    {
        [HttpPost("login")]
        public async Task<ActionResult<UserData>> Login(Login.Execute data)
        {
            return await Mediator.Send(data);
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserData>> Register(UserRegister.Execute data)
        {
            return await Mediator.Send(data);
        }

        [HttpGet]
        public async Task<ActionResult<UserData>> GetUser()
        {
            return await Mediator.Send(new UserLogged.Execute());
        }

        [HttpPut]
        public async Task<ActionResult<UserData>> Update(UserUpdate.Execute parameters)
        {
            return await Mediator.Send(parameters);
        }
    }
}