using System;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.DapperConnection.Instructor;
using MediatR;

namespace Application.Instructor
{
    public class DeleteInstructor
    {
        public class Execute : IRequest
        {
            public Guid InstructorId { get; set; }
        }

        public class Handler : IRequestHandler<Execute>
        {
            private readonly IInstructor _instructorRepository;

            public Handler(IInstructor instructorRepository)
            {
                _instructorRepository = instructorRepository;
            }
            public async Task<Unit> Handle(Execute request, CancellationToken cancellationToken)
            {
                var response = await _instructorRepository.Delete(request.InstructorId);

                if(response > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("Cannot delete instructor");
            }
        }
    }
}