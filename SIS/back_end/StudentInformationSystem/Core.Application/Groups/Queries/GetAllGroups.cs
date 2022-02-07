using AutoMapper;
using Core.Application.Groups.Models;
using Core.Application.Interfaces;
using Core.Application.QueryUtil;
using Core.Domain.Entities;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Guid = System.Guid;

namespace Core.Application.Groups.Queries
{
    public class GetAllGroups : GetAllBase<GroupModel, Group>
    {
        public class Handler : HandlerBase
        {
            private readonly IClaimsValidator claimsValidator;

            public Handler(IRepository<Group> repository, IMapper mapper, IClaimsValidator claimsValidator) 
                : base(repository, mapper)
            {
                this.claimsValidator = claimsValidator ?? throw new ArgumentNullException(nameof(claimsValidator));
            }

            protected override ExpressionStarter<Group> FilterExpression(string filter)
            {
                var predicate = PredicateBuilder.New<Group>(false);
                filter = filter.ToUpper();

                predicate.Or(g => g.Name.ToUpper().Contains(filter)); // should all these be indexed?
                predicate.Or(g => g.Period.ToUpper().Contains(filter));

                return predicate;
            }

            protected override IQueryable<Group> FilterOnPermissions(IQueryable<Group> groups)
            {
                if (claimsValidator.HasGodPermission()) return groups;

                Guid studentId = claimsValidator.GetStudentId();

                if (studentId == default) throw new KeyNotFoundException("Student id not found");

                return groups.Where(g => g.Students.Any(s => s.Id == studentId));
            }
        }
    }
}