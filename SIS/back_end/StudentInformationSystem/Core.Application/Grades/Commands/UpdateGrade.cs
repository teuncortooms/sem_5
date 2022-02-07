using AutoMapper;
using Core.Application.Interfaces;
using Core.Application.Models;
using Core.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Application.Grades.Commands
{
    public static class UpdateGrade
    {
        public class Command : GradeViewModel, IRequest<GradeViewModel>
        {

        }

        public class Handler : IRequestHandler<Command, GradeViewModel>
        {
            private readonly IRepository<Grade> gradeRepository;
            private readonly IMapper mapper;

            public Handler(IRepository<Grade> gradeRepository, IMapper mapper)
            {
                this.gradeRepository = gradeRepository;
                this.mapper = mapper;
            }


            public async Task<GradeViewModel> Handle(Command request, CancellationToken cancellationToken)
            {
                Grade original = mapper.Map<Grade>(request);
                if (original == null) return null;

                Grade updated = await gradeRepository.UpdateAsync(request.Id, original, cancellationToken).ConfigureAwait(false);
                if (updated == null) return null;

                await gradeRepository.CommitAsync(cancellationToken);

                return mapper.Map<GradeViewModel>(updated);
            }
        }
    }
}
