using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.ErrorHandler;
using DataAccess.DapperConnection.Instructor;
using MediatR;

namespace Application.Instructor
{
    public class InstructorById
    {
        public class Execute : IRequest<InstructorModel>
        {
            public Guid InstructorId { get; set; }
        }

        public class Handler : IRequestHandler<Execute, InstructorModel>
        {
            private readonly IInstructor _instructorRepository;

            public Handler(IInstructor instructorRepository)
            {
                _instructorRepository = instructorRepository;
            }

            public async Task<InstructorModel> Handle(Execute request, CancellationToken cancellationToken)
            {
                var response = await _instructorRepository.GetById(request.InstructorId);

                if (response == null)
                {
                    throw new ExceptionHandler(HttpStatusCode.NotFound,"Cannot found instructor");
                }
                
                return response;
            }
        }
    }
}