using System;
using System.Threading.Tasks;
using Application.Commentary;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class CommentaryController : MyBaseController
    {
        [HttpPost]
        public async Task<ActionResult<Unit>> Create(NewCommentary.Execute data)
        {
            return await Mediator.Send(data);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(Guid id)
        {
            return await Mediator.Send(new DeleteCommentary.Execute{CommentaryId = id});
        }
    }
}