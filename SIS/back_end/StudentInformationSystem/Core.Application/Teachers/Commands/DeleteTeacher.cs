using Core.Application.Interfaces;
using Core.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Application.Teachers.Commands
{
    public static class DeleteTeacher
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
            private readonly IRepository<Teacher> teacherRepository;

            public Handler(IRepository<Teacher> teacherRepository)
            {
                this.teacherRepository = teacherRepository ?? throw new ArgumentNullException(nameof(teacherRepository));
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken = default)
            {
                foreach (var id in request.Ids)
                {
                    await teacherRepository.DeleteAsync(id, cancellationToken);
                }

                await teacherRepository.CommitAsync(cancellationToken);

                return Unit.Value;

                // TODO: error handling, mediator should return Response object!
            }
        }
    }
}
