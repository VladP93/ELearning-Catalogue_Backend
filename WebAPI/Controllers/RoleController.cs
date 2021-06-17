using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Security;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class RoleController : MyBaseController
    {
        [HttpGet("list")]
        public async Task<ActionResult<List<IdentityRole>>> List()
        {
            return await Mediator.Send(new RolesQuery.Execute());
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<List<string>>> RolesByUser(string username)
        {
            return await Mediator.Send(new RolesByUser.Execute{UserName = username});
        }

        [HttpPost("create")]
        public async Task<ActionResult<Unit>> Create(NewRole.Execute parameters)
        {
            return await Mediator.Send(parameters);
        }

        [HttpPost("addUserRole")]
        public async Task<ActionResult<Unit>> AddUserRole(AddUserRole.Execute parameters)
        {
            return await Mediator.Send(parameters);
        }

        [HttpPost("deleteUserRole")]
        public async Task<ActionResult<Unit>> DeleteUserRole(DeleteUserRole.Execute parameters)
        {
            return await Mediator.Send(parameters);
        }

        [HttpDelete("delete")]
        public async Task<ActionResult<Unit>> Delete(DeleteRole.Execute parameters)
        {
            return await Mediator.Send(parameters);
        }

    }
}