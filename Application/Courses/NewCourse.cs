using System;
using System.Threading;
using System.Threading.Tasks;
using Business;
using DataAccess;
using FluentValidation;
using MediatR;

namespace Application.Courses
{
    public class NewCourse
    {
        public class Execute : IRequest
        {
            public string Title { get; set; } 
            public string CourseDescription { get; set; }
            public DateTime? PublicationDate { get; set; }
        }

        public class ExecuteValidation : AbstractValidator<Execute>
        {
            public ExecuteValidation()
            {
                RuleFor( x => x.Title ).NotEmpty();
                RuleFor( x => x.CourseDescription ).NotEmpty();
                RuleFor( x => x.PublicationDate ).NotEmpty(); 
            }
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