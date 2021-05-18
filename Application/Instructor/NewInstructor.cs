using System;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.DapperConnection.Instructor;
using FluentValidation;
using MediatR;

namespace Application.Instructor
{
    public class NewInstructor
    {
        public class Execute : IRequest
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Degree { get; set; }
        }

        public class ExecuteValidation : AbstractValidator<Execute>
        {
            public ExecuteValidation()
            {
                RuleFor(i => i.FirstName).NotEmpty();
                RuleFor(i => i.LastName).NotEmpty();
                RuleFor(i => i.Degree).NotEmpty();
            }
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
                var response = await _instructorRepository.Create(request.FirstName, request.LastName, request.Degree);

                if(response>0)
                {
                    return Unit.Value;
                }
                
                throw new Exception("Cannot create new instructor");

            }
        }

    }
}