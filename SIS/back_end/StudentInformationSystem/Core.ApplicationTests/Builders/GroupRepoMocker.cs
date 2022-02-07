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

namespace Core.ApplicationTests.Builders
{
    public class GroupRepoMocker
    {
        private const string GROUPNAME_PREFIX = "Groupname_";
        private const string PERIOD_PREFIX = "Period_";
        private static readonly DateTime STARTDATE_DEFAULT = new(2021, 9, 1);
        private static readonly DateTime ENDDATE_DEFAULT = new(2022, 1, 31); 

        private readonly Mock<IRepository<Group>> repo;
        private List<Group> groups;

        public static GroupRepoMocker GroupRepoMock() => new();

        private GroupRepoMocker()
        {
            repo = new Mock<IRepository<Group>>();
        }

        public GroupRepoMocker WithGroups(IEnumerable<Group> groups)
        {
            this.groups = groups.ToList();
            repo.Setup(e => e.Query()).Returns(groups.AsQueryable());
            repo.Setup(e => e.GetAllAsync(CancellationToken.None)).ReturnsAsync(groups);
            repo.Setup(e => e.GetAsync(It.IsAny<Guid>(), CancellationToken.None)).Returns<Guid, CancellationToken>(
                (id, token) => Task.FromResult(GetGroup(id)));
            return this;
        }

        public GroupRepoMocker WithGroups(int amount)
        {
            var groups = CreateGroups(amount);
            return WithGroups(groups);
        }

        private static IEnumerable<Group> CreateGroups(int dataSize)
        {
            var groups = new List<Group>();

            for (int i = 1; i <= dataSize; i++)
            {
                var group = new Group(
                    Guid.NewGuid(),
                    GROUPNAME_PREFIX + i,
                    PERIOD_PREFIX + i,
                    STARTDATE_DEFAULT.AddMonths(i * 6),
                    ENDDATE_DEFAULT.AddMonths(i * 6));
                groups.Add(group);
            }

            return groups;
        }

        private Group GetGroup(Guid id) => groups.FirstOrDefault(g => g.Id == id);

        public IRepository<Group> Build()
        {
            return repo.Object;
        }
    }
}
