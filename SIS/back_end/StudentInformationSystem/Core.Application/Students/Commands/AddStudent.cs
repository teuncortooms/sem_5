using Core.Application.Interfaces;
using Core.Application.Students.Models;
using Core.Domain;
using Core.Domain.Entities;
using MediatR;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Application.Students.Commands
{
    public static class AddStudent
    {
        public class Command : IRequest<AddStudentModel>
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
        }


        public class Handler : IRequestHandler<Command, AddStudentModel>
        {
            private readonly IRepository<Student> studentRepository;
            private readonly DomainFactory factory;

            public Handler(IRepository<Student> studentRepository, DomainFactory factory)
            {
                this.studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
                this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
            }

            public async Task<AddStudentModel> Handle(Command request, CancellationToken cancellationToken = default)
            {
                if (!IsValidEmail(request.Email)) return null; // TODO: validation pipeline
                
                Student input = factory.CreateStudent(request.FirstName, request.LastName, request.Email);
                studentRepository.Add(input);

                int changes = await studentRepository.CommitAsync(cancellationToken);
                if (changes < 1) return null;

                Student newStudent = input; //TODO:  await studentRepository.GetAllAsync().Find?..... QUERYABLE!

                return new AddStudentModel(newStudent);
            }

            public static bool IsValidEmail(string source)
            {
                return new EmailAddressAttribute().IsValid(source);
            }
        }
    }
}