using Core.Application.Interfaces;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Domain.Entities;
using AutoMapper;
using Core.Application.QueryUtil;
using Core.Application.Models;

namespace Core.Application.Grades.Queries
{
    public static class GetGrades
    {
        public class Query : IRequest<PagedListWrapper<GradeViewModel>>
        {
            public int Page { get; set; }
            public int PageSize { get; set; }
            public string Filter { get; set; }
            public string OrderBy { get; set; }
        }

        public class Handler : IRequestHandler<Query, PagedListWrapper<GradeViewModel>>
        {
            private readonly IRepository<Grade> gradeRepository;
            private readonly IMapper mapper;

            public Handler(IRepository<Grade> gradeRepository, IMapper mapper)
            {
                this.gradeRepository = gradeRepository;
                this.mapper = mapper;
            }

            public async Task<PagedListWrapper<GradeViewModel>> Handle(Query request, CancellationToken cancellationToken)
            {
                var gradeQry = gradeRepository.Query(g=>g.Group,g=>g.Student,g=>g.Student.CurrentGroup).FilterQuery(request);
                var totalRecords = await gradeRepository.ContextAsync(gradeQry).CountAsync(cancellationToken).ConfigureAwait(false);

                gradeQry = gradeQry.PageQuery(request).ApplySort(request.OrderBy);
                var grades = await gradeRepository.ContextAsync(gradeQry).ToListAsync(cancellationToken).ConfigureAwait(false);
                 
                var result = mapper.ProjectTo<GradeViewModel>(grades.AsQueryable());

                return new PagedListWrapper<GradeViewModel> { TotalCount = totalRecords, Results = result.ToList() };
            }

            
        }

        private static IQueryable<Grade> PageQuery(this IQueryable<Grade> query, Query request)
        {
            return query.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize); 
        }

        private static IQueryable<Grade> FilterQuery(this IQueryable<Grade> query, Query request)
        {
            if (string.IsNullOrEmpty(request.Filter))
                return query;

            var filter = request.Filter.ToUpper();
            return query.Where(g=> 
                g.Student.FirstName.ToUpper().Contains(filter) || g.Group.Name.ToUpper().Contains(filter) ||
                g.Student.LastName.ToUpper().Contains(filter)
                );
        }
    }
}
