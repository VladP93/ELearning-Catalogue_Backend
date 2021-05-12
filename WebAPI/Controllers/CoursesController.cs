using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Business;
using MediatR;
using Application.Courses;

namespace WebAPI.Controllers
{
    [ApiController]
    public class CoursesController : MyBaseController
    {
        [HttpGet]
        public async Task<ActionResult<List<Course>>> Get()
        {
            return await Mediator.Send(new CoursesQuery.CoursesList());
        }

        [HttpGet ("{id}")]
        public async Task<ActionResult<Course>> Detail(int id)
        {
            return await Mediator.Send(new CourseById.GetCourse{Id = id});
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Create(NewCourse.Execute data)
        {
            return await Mediator.Send(data);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Update(int id, EditCourse.Execute data)
        {
            data.CourseId = id;
            return await Mediator.Send(data);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(int id)
        {
            return await Mediator.Send(new DeleteCourse.Execute{Id = id});
        }

    }
}