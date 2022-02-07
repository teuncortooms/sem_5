using AutoMapper;
using Core.Application.Interfaces;
using Core.Application.QueryUtil;
using Core.Application.Teachers.Models;
using Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Teachers.Queries
{
    public class GetTeacherById : GetByIdBase<TeacherModel, Teacher>
    {
        public class Handler : HandlerBase
        {
            public Handler(IRepository<Teacher> repository, IMapper mapper) : base(repository, mapper)
            {
            }
        }
    }
}

