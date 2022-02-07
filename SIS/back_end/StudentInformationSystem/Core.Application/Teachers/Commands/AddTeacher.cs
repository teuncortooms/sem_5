using AutoMapper;
using Core.Application.Interfaces;
using Core.Application.Teachers.Models;
using Core.Domain;
using Core.Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Application.Teachers.Commands
{
    public static class AddTeacher
    {
        public class AddTeacherCommand : IRequest<TeacherModel>
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        public class AddTeacherHandler : IRequestHandler<AddTeacherCommand, TeacherModel>
        {
            private readonly IRepository<Teacher> teacherRepository;
            private readonly DomainFactory factory;
            private readonly IMapper mapper;

            public AddTeacherHandler(IRepository<Teacher> teacherRepository, DomainFactory factory, IMapper mapper)
            {
                this.teacherRepository = teacherRepository ?? throw new ArgumentNullException(nameof(teacherRepository));
                this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
                this.mapper = mapper;
            }

            public async Task<TeacherModel> Handle(AddTeacherCommand request, CancellationToken cancellationToken = default)
            {
                Teacher input = factory.CreateTeacher(request.FirstName, request.LastName);
                teacherRepository.Add(input);

                int changes = await teacherRepository.CommitAsync(cancellationToken);
                if (changes < 1) return null;

                return mapper.Map<TeacherModel>(input);
            }

        }
        public class AddTeacherCommandValidator : AbstractValidator<AddTeacherCommand>
        {
            public AddTeacherCommandValidator()
            {
                RuleFor(x => x.FirstName).NotEmpty().WithMessage("First Name cannot be empty");
                RuleFor(x => x.LastName).NotEmpty();
                RuleFor(x => x.LastName).NotEmpty().MaximumLength(15);
            }
        }
    }
}
