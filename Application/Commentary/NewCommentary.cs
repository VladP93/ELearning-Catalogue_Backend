using System;
using System.Threading;
using System.Threading.Tasks;
using DataAccess;
using FluentValidation;
using MediatR;

namespace Application.Commentary
{
    public class NewCommentary
    {
        public class Execute : IRequest
        {
            public string Student { get; set; }
            public int Score { get; set; }
            public string CommentaryText { get; set; }
            public Guid CourseId { get; set; }
        }

        public class ExecuteValidation : AbstractValidator<Execute>
        {
            public ExecuteValidation()
            {
                RuleFor( c => c.Student).NotEmpty();
                RuleFor( c => c.Score).NotEmpty();
                RuleFor( c => c.CommentaryText).NotEmpty();
                RuleFor( c => c.CourseId ).NotEmpty();
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
                var commentary = new Business.Commentary
                {
                    CommentaryId = Guid.NewGuid(),
                    Student = request.Student,
                    Score = request.Score,
                    CommentaryText = request.CommentaryText,
                    CourseId = request.CourseId
                };

                _context.Commentary.Add(commentary);

                var response = await _context.SaveChangesAsync();

                if(response>0)
                {
                    return Unit.Value;
                }

                throw new Exception("Cannot insert commentary");

            }
        }
    }
}