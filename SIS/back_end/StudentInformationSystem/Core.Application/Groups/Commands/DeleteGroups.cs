using Core.Application.Interfaces;
using Core.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Application.Groups.Commands
{
    public static class DeleteGroups
    {
        public class Command : IRequest
        {
            public ICollection<Guid> Ids { get; set; }

            public Command() { } // needed
            public Command(ICollection<Guid> ids) { Ids = ids ?? throw new ArgumentNullException(nameof(ids)); }
            public Command(Guid id) { Ids = new List<Guid>() { id }; }
        }


        class Handler : IRequestHandler<Command>
        {
            private readonly IRepository<Group> groupRepository;

            public Handler(IRepository<Group> groupRepository)
            {
                this.groupRepository = groupRepository ?? throw new ArgumentNullException(nameof(groupRepository));
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken = default)
            {
                foreach (var id in request.Ids)
                {
                    await groupRepository.DeleteAsync(id, cancellationToken);
                }

                await groupRepository.CommitAsync(cancellationToken);

                return Unit.Value;

                // TODO: error handling, mediator should return Response object!
            }
        }
    }
}