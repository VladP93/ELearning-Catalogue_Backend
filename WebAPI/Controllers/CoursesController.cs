using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Business;
using MediatR;
using Application.Courses;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CoursesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<Course>>> Get()
        {
            return await _mediator.Send(new CoursesQuery.CoursesList());
        }

        [HttpGet ("{id}")]
        public async Task<ActionResult<Course>> Detail(int id)
        {
            return await _mediator.Send(new CourseById.GetCourse{Id = id});
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Create(NewCourse.Execute data)
        {
            return await _mediator.Send(data);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Update(int id, EditCourse.Execute data)
        {
            data.CourseId = id;
            return await _mediator.Send(data);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(int id)
        {
            return await _mediator.Send(new DeleteCourse.Execute{Id = id});
        }

    }
}