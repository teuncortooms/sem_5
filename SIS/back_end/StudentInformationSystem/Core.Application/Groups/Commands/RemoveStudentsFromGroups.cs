using Core.Application.Interfaces;
using Core.Domain;
using Core.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Application.Groups.Commands
{
    public static class RemoveStudentsFromGroups
    {
        public class Command : IRequest<int>
        {
            public ICollection<Guid> StudentIds { get; set; }
            public ICollection<Guid> GroupIds { get; set; }
        }


        public class Handler : IRequestHandler<Command, int>
        {
            private readonly ILogger<Handler> logger;
            private readonly IRepository<Group> groupRepository;
            private readonly IRepository<Student> studentRepository;
            private readonly DomainFactory factory;

            public Handler(ILogger<Handler> logger,
                IRepository<Group> groupRepository, IRepository<Student> studentRepository,
                DomainFactory factory)
            {
                this.logger = logger;
                this.groupRepository = groupRepository ?? throw new ArgumentNullException(nameof(groupRepository));
                this.studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
                this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken = default)
            {
                int changes = 0;
                try
                {
                    if (IsInvalid(request)) throw new ArgumentException("Request has invalid properties.");

                    List<Student> students = await GetById<Student>(request.StudentIds, cancellationToken);
                    if (students == null) throw new NullReferenceException("Could not find students.");

                    List<Group> groups = await GetById<Group>(request.GroupIds, cancellationToken);
                    if (groups == null) throw new NullReferenceException("Could not find groups.");

                    bool success = RemoveChildrenFromManyParents(groups, students);
                    if (!success) throw new NullReferenceException("There was an error updating students.");

                    changes = await groupRepository.CommitAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    logger.LogDebug("An error occurred: " + ex.Message);
                    await groupRepository.DisposeAsync();
                }
                return changes;
            }

            private static bool IsInvalid(Command request)
            {
                return request == null || request.StudentIds == null || request.GroupIds == null ||
                                request.StudentIds.Count <= 0 || request.GroupIds.Count <= 0;
            }

            private async Task<List<T>> GetById<T>(ICollection<Guid> ids, CancellationToken cancellationToken)
            {
                var repo = GetRepo<T>();
                List<T> items = new();
                foreach (var id in ids)
                {
                    var item = await repo.GetAsync(id, cancellationToken);
                    if (item == null) throw new ArgumentException($"{nameof(T)} {id} not found.");
                    items.Add(item);
                }
                return items;
            }

            private IRepository<T> GetRepo<T>()
            {
                if (typeof(T) == typeof(Group)) return this.groupRepository as IRepository<T>;
                if (typeof(T) == typeof(Student)) return this.studentRepository as IRepository<T>;
                throw new ArgumentException("Type unknown");
            }

            private static bool RemoveChildrenFromManyParents<TChild, TParent>(List<TChild> children, List<TParent> parents)
            {
                foreach (var p in parents)
                {
                    bool success = RemoveChildren(children, p);
                    if (!success) return false;
                }
                return true;
            }

            private static bool RemoveChildren<TChild, TParent>(List<TChild> children, TParent parent)
            {
                if (typeof(TChild) == typeof(Group) && typeof(TParent) == typeof(Student))
                    return RemoveStudents(children as List<Group>, parent as Student);

                throw new ArgumentException("Types unknown");
            }

            private static bool RemoveStudents(List<Group> groups, Student student)
            {
                foreach (var groupToRemove in groups)
                {
                    bool success = student.RemoveGroup(groupToRemove.Id);
                    if (!success) return false;
                }
                return true;
            }
        }
    }
}