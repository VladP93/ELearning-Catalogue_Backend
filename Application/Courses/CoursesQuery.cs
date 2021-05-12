using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Business;
using DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Courses
{
    public class CoursesQuery
    {
        public class CoursesList : IRequest<List<Course>>{}

        public class Handler : IRequestHandler<CoursesList, List<Course>>
        {
            private readonly ElearningCatalogContext _context;

            public Handler(ElearningCatalogContext context)
            {
                _context = context;
            }
            public async Task<List<Course>> Handle(CoursesList request, CancellationToken cancellationToken)
            {
                var courses = await _context.Course.ToListAsync();
                return courses;
            }
        }
    }
}