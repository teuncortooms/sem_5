using Core.Application.Groups.Models;
using Core.Application.Interfaces;
using Core.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Application.Groups.Commands
{
    public static class UpdateGroup
    {
        public class Command : IRequest<GroupModel>
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Period { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
        }


        public class Handler : IRequestHandler<Command, GroupModel>
        {
            private readonly IRepository<Group> groupRepository;

            public Handler(IRepository<Group> groupRepository)
            {
                this.groupRepository = groupRepository ?? throw new ArgumentNullException(nameof(groupRepository));
            }

            public async Task<GroupModel> Handle(Command request, CancellationToken cancellationToken = default)
            {
                Group original = await groupRepository.GetAsync(request.Id, cancellationToken);
                if (original == null) return null;

                Group updated = await groupRepository.UpdateAsync(request.Id, request, cancellationToken);
                if (updated == null) return null;

                await groupRepository.CommitAsync(cancellationToken);

                return new GroupModel(updated);
            }
        }
    }
}