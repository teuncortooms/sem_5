using Core.Application.Interfaces;
using Core.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Application.Students.Models;
using Core.Application.QueryUtil;
using AutoMapper;

namespace Core.Application.Students.Queries
{
    public class GetStudentDetails : GetByIdBase<StudentDetailsModel, Student>
    {
        public class Handler : HandlerBase
        {
            public Handler(IRepository<Student> repository, IMapper mapper) : base(repository, mapper)
            {
            }
        }
    }
}