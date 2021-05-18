using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.DTO;
using AutoMapper;
using Business;
using DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Course
{
    public class CoursesQuery
    {
        public class CoursesList : IRequest<List<CourseDTO>>{}

        public class Handler : IRequestHandler<CoursesList, List<CourseDTO>>
        {
            private readonly ElearningCatalogContext _context;
            private readonly IMapper _mapper;

            public Handler(ElearningCatalogContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<List<CourseDTO>> Handle(CoursesList request, CancellationToken cancellationToken)
            {
                var courses = await _context.Course
                .Include(c => c.CourseInstructors)
                .ThenInclude(ci =>ci.Instructor)
                .Include(co => co.Commentaries)
                .Include(p => p.Price )
                .ToListAsync();

                var coursesDTO = _mapper.Map<List<Business.Course>, List<CourseDTO>>(courses);

                return coursesDTO;
            }
        }
    }
}