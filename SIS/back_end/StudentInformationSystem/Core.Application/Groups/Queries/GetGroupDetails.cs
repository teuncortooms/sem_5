using AutoMapper;
using Core.Application.Groups.Models;
using Core.Application.Interfaces;
using Core.Application.QueryUtil;
using Core.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Application.Groups.Queries
{
    public class GetGroupDetails : GetByIdBase<GroupDetailsModel, Group>
    {
        public class Handler : HandlerBase
        {
            public Handler(IRepository<Group> repository, IMapper mapper) : base(repository, mapper)
            {
            }
        }
    }
}