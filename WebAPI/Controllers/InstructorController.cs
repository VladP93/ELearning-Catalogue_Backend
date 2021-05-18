

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Instructor;
using DataAccess.DapperConnection.Instructor;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class InstructorController : MyBaseController
    {
        [HttpGet]
        public async Task<ActionResult<List<InstructorModel>>> GetInstructors()
        {
            return await Mediator.Send(new InstructorsQuery.InstructorList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InstructorModel>> GetInstructorById(Guid id)
        {
            return await Mediator.Send(new InstructorById.Execute{InstructorId = id});
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Create(NewInstructor.Execute data)
        {
            return await Mediator.Send(data);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Update(Guid id, EditInstructor.Execute data)
        {
            data.InstructorId = id;
            return await Mediator.Send(data);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(Guid id)
        {
            return await Mediator.Send(new DeleteInstructor.Execute{InstructorId = id});
        }
    }
}