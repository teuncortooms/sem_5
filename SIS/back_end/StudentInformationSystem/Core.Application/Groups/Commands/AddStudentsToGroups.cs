using Core.Application.Interfaces;
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
    public static class AddStudentsToGroups
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

            public Handler(ILogger<Handler> logger,
                IRepository<Group> groupRepository, IRepository<Student> studentRepository)
            {
                this.logger = logger;
                this.groupRepository = groupRepository ?? throw new ArgumentNullException(nameof(groupRepository));
                this.studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken = default)
            {
                int changes = 0;
                try
                {
                    if (IsInvalid(request)) throw new ArgumentException("Request has invalid properties.");

                    List<Student> students = await GetStudents(request, cancellationToken);
                    if (students == null) throw new NullReferenceException("Could not find students.");

                    List<Group> groups = await GetGroups(request, cancellationToken);
                    if (groups == null) throw new NullReferenceException("Could not find groups.");

                    AddStudentsToGroups(groups, students);

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

            private async Task<List<Student>> GetStudents(Command request, CancellationToken cancellationToken)
            {
                List<Student> students = new();
                foreach (var studentId in request.StudentIds)
                {
                    var student = await studentRepository.GetAsync(studentId, cancellationToken);
                    if (student == null) throw new ArgumentException($"Student {studentId} not found.");
                    students.Add(student);
                }
                return students;
            }

            private async Task<List<Group>> GetGroups(Command request, CancellationToken cancellationToken)
            {
                List<Group> groups = new();
                foreach (var groupId in request.GroupIds)
                {
                    var group = await groupRepository.GetAsync(groupId, cancellationToken);
                    if (group == null) throw new ArgumentException($"Group {group.Name} ({group.Id}) not found.");
                    groups.Add(group);
                }
                return groups;
            }

            private static void AddStudentsToGroups(List<Group> groups, List<Student> students)
            {
                foreach (var group in groups)
                {
                    AddStudentsToGroup(group, students);
                }
            }

            private static void AddStudentsToGroup(Group group, List<Student> students)
            {
                foreach (var student in students)
                {
                    if (student.Groups.Contains(group)) continue;
                    // TODO: validate if student has another group on the same date period
                    student.AddGroup(group);
                    //group.Students.Add(student);
                }
            }
        }
    }
}