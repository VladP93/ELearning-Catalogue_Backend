using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.DTO;
using Application.ErrorHandler;
using AutoMapper;
using Business;
using DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Course
{
    public class CourseById
    {
        
        public class GetCourse : IRequest<CourseDTO>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<GetCourse, CourseDTO>
        {
            private readonly ElearningCatalogContext _context;
            private readonly IMapper _mapper;

            public Handler(ElearningCatalogContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<CourseDTO> Handle(GetCourse request, CancellationToken cancellationToken)
            {
                var course = await _context.Course
                .Include(c => c.CourseInstructors)
                .ThenInclude(ci =>ci.Instructor)
                .Include(co => co.Commentaries)
                .Include(p => p.Price )
                .FirstOrDefaultAsync(p => p.CourseId == request.Id);

                if (course == null)
                {
                    throw new ExceptionHandler(HttpStatusCode.NotFound, new {Code = HttpStatusCode.NotFound, CourseError = "Course not found"});
                }

                var courseDTO = _mapper.Map<Business.Course, CourseDTO>(course);

                return courseDTO;
            }
        }

    }
}