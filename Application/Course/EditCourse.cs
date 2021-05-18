using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.ErrorHandler;
using Business;
using DataAccess;
using FluentValidation;
using MediatR;

namespace Application.Course
{
    public class EditCourse
    {
        public class Execute : IRequest
        {
            public Guid CourseId { get; set; }
            public string Title { get; set; } 
            public string CourseDescription { get; set; }
            public DateTime? PublicationDate { get; set; }
            public List<Guid> Instructors { get; set; }
            public decimal? Price { get; set; }
            public decimal? PromotionPrice { get; set; }
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
                var course = await _context.Course.FindAsync(request.CourseId);

                if (course == null)
                {
                    throw new ExceptionHandler(HttpStatusCode.NotFound, new {Code = HttpStatusCode.NotFound, CourseError = "Course not found"});
                }

                course.Title = request.Title ?? course.Title;
                course.CourseDescription = request.CourseDescription ?? course.CourseDescription;
                course.PublicationDate = request.PublicationDate ?? course.PublicationDate;

                var priceEntity = _context.Price.Where(c => c.CourseId == course.CourseId).FirstOrDefault();
                if(priceEntity!=null)
                {
                    priceEntity.ActualPrice = request.Price ?? priceEntity.ActualPrice;
                    priceEntity.PromotionPrice = request.PromotionPrice ?? priceEntity.PromotionPrice;
                }
                else
                {
                    priceEntity = new Price
                    {
                        PriceId = Guid.NewGuid(),
                        ActualPrice = request.Price ?? 0,
                        PromotionPrice = request.PromotionPrice ?? 0,
                        CourseId = course.CourseId
                    };
                    
                    await _context.Price.AddAsync(priceEntity);
                }

                if(request.Instructors!=null)
                {
                    if(request.Instructors.Count>0)
                    {
                        var actualInstructors = _context.CourseInstructor.Where(ci => ci.CourseId == request.CourseId).ToList();
                        foreach(var guid in actualInstructors)
                        {
                            _context.CourseInstructor.Remove(guid);
                        }

                        foreach(var guid in request.Instructors)
                        {
                            var newInstructor = new CourseInstructor
                            {
                                CourseId = request.CourseId,
                                InstructorId = guid     
                            };
                            _context.CourseInstructor.Add(newInstructor);
                        }

                    }
                }

                var response = await _context.SaveChangesAsync();

                if (response <= 0)
                    throw new Exception("Error: Cannot modify course");
                return Unit.Value;
            }
        }
    }
}