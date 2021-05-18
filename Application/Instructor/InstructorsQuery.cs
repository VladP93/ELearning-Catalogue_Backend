using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.DapperConnection.Instructor;
using MediatR;

namespace Application.Instructor
{
    public class InstructorsQuery 
    {
        public class InstructorList: IRequest<List<InstructorModel>>{}
        public class Handler : IRequestHandler<InstructorList, List<InstructorModel>>
        {

            private readonly IInstructor _instructorRepository;

            public Handler(IInstructor instructorRepository)
            {
                _instructorRepository = instructorRepository;
            }

            public async Task<List<InstructorModel>> Handle(InstructorList request, CancellationToken cancellationToken)
            {
                var result = await _instructorRepository.GetList();
                return result.ToList();
            }
        }
    }
}