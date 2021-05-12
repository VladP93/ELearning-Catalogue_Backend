using System;
using System.Threading;
using System.Threading.Tasks;
using Business;
using DataAccess;
using MediatR;

namespace Application.Courses
{
    public class NewCourse
    {
        public class Execute : IRequest
        {
            public string Title { get; set; } 
            public string CourseDescription { get; set; }
            public DateTime PublicationDate { get; set; }
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
                var course = new Course
                {
                    Title = request.Title,
                    CourseDescription = request.CourseDescription,
                    PublicationDate = request.PublicationDate
                };

                _context.Course.Add(course);
                var response = await _context.SaveChangesAsync();

                if (response <= 0)
                    throw new Exception("Error: Cannot insert the course");
                return Unit.Value;
            }
        }

    }
}