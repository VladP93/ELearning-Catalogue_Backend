using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.ErrorHandler;
using Business;
using DataAccess;
using MediatR;

namespace Application.Courses
{
    public class CourseById
    {
        
        public class GetCourse : IRequest<Course>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<GetCourse, Course>
        {
            private readonly ElearningCatalogContext _context;

            public Handler(ElearningCatalogContext context)
            {
                _context = context;
            }

            public async Task<Course> Handle(GetCourse request, CancellationToken cancellationToken)
            {
                var course = await _context.Course.FindAsync(request.Id);

                if (course == null)
                {
                    throw new ExceptionHandler(HttpStatusCode.NotFound, new {Code = HttpStatusCode.NotFound, CourseError = "Course not found"});
                }

                return course;
            }
        }

    }
}