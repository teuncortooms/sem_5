using Core.Application.Interfaces;
using Core.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Application.Students.Models;

namespace Core.Application.Students.Commands
{
    public static class UpdateStudent
    {
        public class Command : IRequest<UpdateStudentModel>
        {
            public Guid Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
        }


        public class Handler : IRequestHandler<Command, UpdateStudentModel>
        {
            private readonly IRepository<Student> studentRepository;

            public Handler(IRepository<Student> studentRepository)
            {
                this.studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
            }

            public async Task<UpdateStudentModel> Handle(Command request, CancellationToken cancellationToken = default)
            {
                Student original = await studentRepository.GetAsync(request.Id, cancellationToken);
                if (original == null) return null;

                Student updated = await studentRepository.UpdateAsync(request.Id, request, cancellationToken);
                if (updated == null) return null;

                await studentRepository.CommitAsync(cancellationToken);

                return new UpdateStudentModel(updated);
            }
        }
    }
}