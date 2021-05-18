using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Business;
using DataAccess;
using FluentValidation;
using MediatR;

namespace Application.Course
{
    public class NewCourse
    {
        public class Execute : IRequest
        {
            public string Title { get; set; } 
            public string CourseDescription { get; set; }
            public DateTime? PublicationDate { get; set; }
            public List<Guid> Instructors {get;set;}
            public decimal Price { get; set; }
            public decimal PromotionPrice { get; set; }
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
                Guid _courseId = Guid.NewGuid();

                var course = new Business.Course
                {
                    CourseId = _courseId,
                    Title = request.Title,
                    CourseDescription = request.CourseDescription,
                    PublicationDate = request.PublicationDate,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Course.Add(course);

                if(request.Instructors!=null)
                {
                    foreach (var id in request.Instructors)
                    {
                        var courseInstructor = new CourseInstructor
                        {
                            CourseId = _courseId,
                            InstructorId = id
                        };
                        _context.CourseInstructor.Add(courseInstructor);
                    }
                }

                var priceEntity = new Price
                {
                    CourseId = _courseId,
                    ActualPrice = request.Price,
                    PromotionPrice = request.PromotionPrice,
                    PriceId = Guid.NewGuid()
                };

                _context.Price.Add(priceEntity);

                var response = await _context.SaveChangesAsync();

                if (response <= 0)
                    throw new Exception("Error: Cannot insert the course");
                return Unit.Value;
            }
        }

    }
}