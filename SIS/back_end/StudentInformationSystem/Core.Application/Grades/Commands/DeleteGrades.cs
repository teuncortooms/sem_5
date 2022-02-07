using Core.Application.Interfaces;
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
    static public class DeleteGrades
    {
        public class Command : IRequest
        {
            public ICollection<Guid> Ids { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly IRepository<Grade> gradeRepository;

            public Handler(IRepository<Grade> gradeRepository)
            {
                this.gradeRepository = gradeRepository;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                foreach (var id in request.Ids)
                {
                    await gradeRepository.DeleteAsync(id, cancellationToken);
                }

                await gradeRepository.CommitAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
