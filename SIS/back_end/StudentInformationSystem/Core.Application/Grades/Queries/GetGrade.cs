using AutoMapper;
using Core.Application.Interfaces;
using Core.Application.Models;
using Core.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Application.Grades.Queries
{
    public static class GetGrade
    {
        public class Query : IRequest<GradeViewModel>
        {
            public Guid id { get; set; }
        }

        public class Handler : IRequestHandler<Query, GradeViewModel>
        {
            private readonly IRepository<Grade> _gradeRepository;
            private readonly IMapper _mapper;
            private readonly HttpContext httpContext;

            public Handler(IRepository<Grade> gradeRepository, IMapper mapper, IHttpContextAccessor httpContext)
            {
                _gradeRepository = gradeRepository;
                _mapper = mapper;
                this.httpContext = httpContext.HttpContext;
            }

            public async Task<GradeViewModel> Handle(Query request, CancellationToken cancellationToken)
            {
                if (!httpContext.User.HasClaim(c => c.Value.Equals("p_grade_read_own") || c.Value.Equals("p_grade_read_all")
                    || c.Value.Equals("p_all")))
                    throw new Exception("Permission denied (no access to grades)");

                var result = await _gradeRepository.ContextAsync(
                    _gradeRepository
                    .Query(m => m.Group, m => m.Student, m => m.Student.CurrentGroup)
                    .Where(m => m.Id.Equals(request.id))
                    ).SingleAsync(cancellationToken)
                    .ConfigureAwait(false);

                var resultView = _mapper.Map<GradeViewModel>(result);
                return resultView;
            }
        }
    }
}
