using AutoMapper;
using Core.Application.Interfaces;
using Core.Application.QueryUtil;
using Core.Application.Teachers.Models;
using Core.Domain.Entities;
using LinqKit;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Application.Teachers.Queries
{
    public  class GetTeachers : GetAllBase<TeacherModel, Teacher>
    {
        
        public class Handler : HandlerBase
        {
            public Handler(IRepository<Teacher> repository, IMapper mapper)
                : base(repository, mapper) { }

            protected override ExpressionStarter<Teacher> FilterExpression(string filter)
            {
                var predicate = PredicateBuilder.New<Teacher>(false);
                filter = filter.ToUpper();

                predicate.Or(s => s.FirstName.ToUpper().Contains(filter));
                predicate.Or(s => s.LastName.ToUpper().Contains(filter));
                
                return predicate;
            }
        }
    }
}
