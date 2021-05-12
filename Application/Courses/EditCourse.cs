using System;
using System.Threading;
using System.Threading.Tasks;
using DataAccess;
using MediatR;

namespace Application.Courses
{
    public class EditCourse
    {
        public class Execute : IRequest
        {
            public int CourseId { get; set; }
            public string Title { get; set; } 
            public string CourseDescription { get; set; }
            public DateTime? PublicationDate { get; set; }
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
                var course = await _context.Course.FindAsync(request.CourseId);

                if (course == null)
                {
                    throw new Exception("Error: Course not found");
                }

                course.Title = request.Title ?? course.Title;
                course.CourseDescription = request.CourseDescription ?? course.CourseDescription;
                course.PublicationDate = request.PublicationDate ?? course.PublicationDate;

                var response = await _context.SaveChangesAsync();

                if (response <= 0)
                    throw new Exception("Error: Cannot modify course");
                return Unit.Value;
            }
        }
    }
}