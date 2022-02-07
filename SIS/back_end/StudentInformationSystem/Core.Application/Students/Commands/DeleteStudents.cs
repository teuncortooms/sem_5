using Core.Application.Interfaces;
using Core.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Application.Students.Commands
{
    public static class DeleteStudents
    {
        public class Command : IRequest
        {
            public ICollection<Guid> Ids { get; set; }

            public Command() { } // needed
            public Command(ICollection<Guid> ids) { Ids = ids ?? throw new ArgumentNullException(nameof(ids)); }
            public Command(Guid id) { Ids = new List<Guid>() { id }; }
        }


        public class Handler : IRequestHandler<Command>
        {
            private readonly IRepository<Student> studentRepository;

            public Handler(IRepository<Student> studentRepository)
            {
                this.studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken = default)
            {
                foreach (var id in request.Ids)
                {
                    await studentRepository.DeleteAsync(id, cancellationToken);
                }

                await studentRepository.CommitAsync(cancellationToken);

                return Unit.Value;

                // TODO: error handling, mediator should return Response object!
            }
        }
    }
}