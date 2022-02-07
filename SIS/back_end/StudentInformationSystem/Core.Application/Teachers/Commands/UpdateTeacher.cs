using AutoMapper;
using Core.Application.Interfaces;
using Core.Application.Teachers.Models;
using Core.Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Application.Teachers.Commands
{
    public static class UpdateTeacher
    {
        public class Command : IRequest<TeacherModel>
        {
            public Guid Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }


        public class Handler : IRequestHandler<Command, TeacherModel>
        {
           // private readonly IValidator<Command> validator;
            private readonly IRepository<Teacher> teacherRepository;
            private readonly IMapper mapper;

            public Handler(IRepository<Teacher> teacherRepository, IMapper mapper)
            {
               // this.validator = validator;
                this.teacherRepository = teacherRepository ?? throw new ArgumentNullException(nameof(teacherRepository));
                this.mapper = mapper;
            }

            public async Task<TeacherModel> Handle(Command request, CancellationToken cancellationToken = default )
            {
                Teacher original = await teacherRepository.GetAsync(request.Id, cancellationToken);
                if (original == null) return null;

                Teacher updated = await teacherRepository.UpdateAsync(request.Id, request, cancellationToken);
                if (updated == null) return null;
   //             validator.ValidateAndThrow(request);
                await teacherRepository.CommitAsync(cancellationToken);

                return mapper.Map<TeacherModel>(updated);
            }
        }
        public class UpdateUserCommandValidator : AbstractValidator<Command>
        {
            public UpdateUserCommandValidator()
            {
                RuleFor(x => x.Id).NotEmpty();
                RuleFor(x => x.FirstName).NotEmpty().WithMessage("First Name cannot be empty");
                RuleFor(x => x.LastName).NotEmpty().MaximumLength(15);
            }
        }

    }
}
