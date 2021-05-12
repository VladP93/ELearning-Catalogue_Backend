using System.Threading.Tasks;
using Application.Security;
using Business;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class UserController : MyBaseController
    {
        [HttpPost("login")]
        public async Task<ActionResult<UserData>> Login(Login.Execute data)
        {
            return await Mediator.Send(data);
        }
    }
}