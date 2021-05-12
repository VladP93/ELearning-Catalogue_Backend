using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.ErrorHandler;
using DataAccess;
using MediatR;

namespace Application.Courses
{
    public class DeleteCourse
    {
        public class Execute : IRequest 
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Execute>
        {
            private readonly ElearningCatalogContext _context;
            public Handler(ElearningCatalogContext context)
            {
                _context = context; 
            }
            public async Task<Unit> Handle(Execute request, CancellationToken cancellationToken)
            {
                var course = await _context.Course.FindAsync(request.Id);

                if(course==null){
                    // throw new Exception("Error: Course not found");
                    throw new ExceptionHandler(HttpStatusCode.NotFound, new {Code = HttpStatusCode.NotFound, CourseError = "Course not found"});
                }

                _context.Remove(course);
                var response = await _context.SaveChangesAsync();

                if(response<=0)
                    throw new Exception("Error: Course cannot be deleted");
                return Unit.Value;
            }
        }
    }
}