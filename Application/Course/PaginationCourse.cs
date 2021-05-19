using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.DapperConnection.Pagination;
using MediatR;

namespace Application.Course
{
    public class PaginationCourse
    {
        public class Execute : IRequest<PaginationModel>
        {
            public string Title { get; set; }
            public int PageNumber { get; set; }
            public int Quantity { get; set; }
        }

        public class Handler : IRequestHandler<Execute, PaginationModel>
        {
            public readonly IPagination _pagination;
            public Handler(IPagination pagination)
            {
                _pagination = pagination;
            }

            public Task<PaginationModel> Handle(Execute request, CancellationToken cancellationToken)
            {
                var storedProcedure = "usp_get_course_pagination";
                var columnSort = "Title";
                var parameters = new Dictionary<string, object>();
                parameters.Add("@courseTitle",request.Title);
                
                return _pagination.returnPagination(storedProcedure, request.PageNumber, request.Quantity, parameters, columnSort);
            }
        }
    }
}