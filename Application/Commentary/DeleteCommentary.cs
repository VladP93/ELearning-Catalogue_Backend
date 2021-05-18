using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.ErrorHandler;
using DataAccess;
using FluentValidation;
using MediatR;

namespace Application.Commentary
{
    public class DeleteCommentary
    {
        public class Execute : IRequest
        {
            public Guid CommentaryId { get; set; }  
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
                var commentary = await _context.Commentary.FindAsync(request.CommentaryId);

                if(commentary == null)
                {
                    throw new ExceptionHandler(HttpStatusCode.NotFound,"Commentary not found");
                }

                _context.Remove(commentary);

                var response = await _context.SaveChangesAsync();

                if(response>0)
                {
                    return Unit.Value;
                }

                throw new Exception("Cannot delete commentary");

            }
        }
    }
}