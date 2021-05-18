using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Business;
using MediatR;
using Application.Course;
using Application.DTO;
using System;

namespace WebAPI.Controllers
{
    [ApiController]
    public class CourseController : MyBaseController
    {
        [HttpGet]
        public async Task<ActionResult<List<CourseDTO>>> Get()
        {
            return await Mediator.Send(new CoursesQuery.CoursesList());
        }

        [HttpGet ("{id}")]
        public async Task<ActionResult<CourseDTO>> Detail(Guid id)
        {
            return await Mediator.Send(new CourseById.GetCourse{Id = id});
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Create(NewCourse.Execute data)
        {
            return await Mediator.Send(data);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Update(Guid id, EditCourse.Execute data)
        {
            data.CourseId = id;
            return await Mediator.Send(data);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(Guid id)
        {
            return await Mediator.Send(new DeleteCourse.Execute{Id = id});
        }

    }
}