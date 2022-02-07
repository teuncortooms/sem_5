using System.Data.Entity;
using System.Linq;
using Core.Application.Interfaces;
using Core.Domain.Entities;
using Core.Application.Students.Models;
using Core.Application.QueryUtil;
using AutoMapper;
using LinqKit;

namespace Core.Application.Students.Queries
{
    public class GetAllStudents : GetAllBase<StudentModel, Student>
    {
        public class Handler : HandlerBase
        {
            public Handler(IRepository<Student> repository, IMapper mapper)
                : base(repository, mapper) { }

            protected override ExpressionStarter<Student> FilterExpression(string filter)
            {
                var predicate = PredicateBuilder.New<Student>(false);
                filter = filter.ToUpper();

                predicate.Or(s => s.FirstName.ToUpper().Contains(filter));
                predicate.Or(s => s.LastName.ToUpper().Contains(filter));
                predicate.Or(s => s.CurrentGroup.Name.ToUpper().Contains(filter));

                return predicate;
            }
        }
    }
}