using Core.Application.Groups.Models;
using Core.Application.Interfaces;
using Core.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Application.Groups.Commands
{
    public static class AddGroup
    {
        public class Command : IRequest<GroupModel>
        {
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

            public async Task<GroupModel> Handle(Command command, CancellationToken cancellationToken = default)
            {
                Group input = new(command.Name, command.Period, command.StartDate, command.EndDate);
                groupRepository.Add(input);

                int changes = await groupRepository.CommitAsync(cancellationToken);
                if (changes < 1) return null;

                Group newGroup = input; //TODO:  await groupRepository.GetAllAsync().F..... QUERYABLE!

                return new GroupModel(newGroup);
            }
        }
    }
}