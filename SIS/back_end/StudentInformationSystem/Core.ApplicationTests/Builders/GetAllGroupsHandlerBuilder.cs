using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Core.Application.Extensions;
using Core.Application.Groups.Models;
using Core.Application.Groups.Queries;
using Core.Application.Interfaces;
using Core.Application.Profiles;
using Core.Domain.Entities;
using MediatR;
using Moq;
using static Core.ApplicationTests.Builders.GroupRepoMocker;
using static Core.ApplicationTests.Builders.MapperBuilder;

namespace Core.ApplicationTests.Builders
{
    public class GetAllGroupsHandlerBuilder
    {
        private readonly IClaimsValidator claimsValidator;
        private readonly IMapper mapper;
        private IRepository<Group> groupRepo;

        public static GetAllGroupsHandlerBuilder DefaultHandler() => new();

        private GetAllGroupsHandlerBuilder()
        {
            groupRepo = GroupRepoMock().Build();
            mapper = DefaultMapper().Build();
            var validator = new Mock<IClaimsValidator>();
            validator.Setup(v => v.HasGodPermission()).Returns(true);
            claimsValidator = validator.Object;
        }

        public GetAllGroupsHandlerBuilder WithGroupsRepo(IRepository<Group> groupsRepo)
        {
            this.groupRepo = groupsRepo;
            return this;
        }

        public GetAllGroups.Handler Build()
        {
            return new GetAllGroups.Handler(groupRepo, mapper, claimsValidator);
        }
    }
}
