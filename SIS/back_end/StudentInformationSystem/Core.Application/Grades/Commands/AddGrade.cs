using AutoMapper;
using Core.Application.Groups.Models;
using Core.Application.Interfaces;
using Core.Application.Models;
using Core.Application.Students.Models;
using Core.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Core.Application.Grades.Commands
{
    public static class AddGrade
    {
        public class Command : IRequest<GradeViewModel>
        {
            public StudentViewModel Student { get; set; }
            public GroupViewModel Group { get; set; }
            public int Score { get; set; }
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, GradeViewModel>
        {
            private readonly IRepository<Grade> _gradeRepository;
            private readonly IMapper _mapper;
            private readonly IRepository<Group> _groupRepository;
            private readonly IRepository<Student> _studentRepository;

            public Handler(IRepository<Grade> gradeRepository, IMapper mapper, IRepository<Group> groupRepository, IRepository<Student> studentRepository)
            {
                _mapper = mapper;
                _groupRepository = groupRepository;
                _studentRepository = studentRepository;
                _gradeRepository = gradeRepository;
            }

            public async Task<GradeViewModel> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = new Grade
                {
                    GroupId = request.Group.Id,
                    StudentId = request.Student.Id,
                    Score = request.Score,
                    Id = Guid.NewGuid()
                };

                using (var scope = new TransactionScope(TransactionScopeOption.Required,
                    new TransactionOptions() { Timeout = TimeSpan.FromSeconds(600), IsolationLevel = IsolationLevel.RepeatableRead },
                    TransactionScopeAsyncFlowOption.Enabled))
                {    
                    _gradeRepository.Add(entity);
                    await _gradeRepository.CommitAsync(cancellationToken).ConfigureAwait(false);
                    scope.Complete();
                }
                entity = _gradeRepository.Query(g=>g.Student,g=>g.Group,g=>g.Student.CurrentGroup).Where(g=>g.Id==entity.Id).First();
                return _mapper.Map<GradeViewModel>(entity); 

            }
        }
    }
}
