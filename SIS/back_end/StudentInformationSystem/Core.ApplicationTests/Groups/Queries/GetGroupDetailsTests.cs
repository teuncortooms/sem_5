using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Moq;
using Core.Application.Interfaces;
using System.Threading;
using Core.Domain.Entities;
using FluentAssertions;
using Core.Domain;
using Microsoft.Extensions.Logging;
using Core.Application.Groups.Models;
using Core.Application.Students.Models;
using AutoMapper;
using Core.Application.Extensions;
using Core.Application.Profiles;
using Core.ApplicationTests;
using Core.ApplicationTests.Builders;
using static Core.ApplicationTests.Builders.GroupRepoMocker;
using static Core.ApplicationTests.Builders.MapperBuilder;
using System.Linq;

namespace Core.Application.Groups.Queries.Tests
{
    [TestClass()]
    public class GetGroupDetailsTests
    {
        [TestMethod()]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(10)]
        public async void Handler_ReturnsGroupWithCorrectPropertiesAndStudents(int groupSize)
        {
            //Arrange
            var repo = GroupRepoMock().WithGroups(groupSize).Build();
            var mapper = DefaultMapper().Build();
            var handler = new GetGroupDetails.Handler(repo, mapper);

            var group = (await repo.GetAllAsync()).ElementAt(0);
            var expected = mapper.Map<GroupDetailsModel>(group);

            // Act
            var actual = handler.Handle(new GetGroupDetails.Query(group.Id)).Result;

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        //[TestMethod()]
        //public void Request_WithUnknownID_ReturnsNull()
        //{
        //    asdfasdf
        //    //Arrange
        //    Guid id = Guid.NewGuid();
        //    GetGroupDetails.Handler handler = GetHandlerWithEmptyRepoAndMapper();

        //    // Act
        //    var actual = handler.Handle(new GetGroupDetails.Query(id)).Result;

        //    // Assert
        //    actual.Should().BeNull();
        //}

        //private static GetGroupDetails.Handler GetDefaultHandler(Guid[] studentIds)
        //{
        //    var repo = GetRepo(studentIds);
        //    var mapper = GetMapper();

        //    return new GetGroupDetails.Handler(repo.Object, mapper);
        //}

        //private static Mock<IRepository<Group>> GetRepo(Guid[] studentIds)
        //{
        //    var repo = new Mock<IRepository<Group>>();
        //    repo.Setup(e => e.GetAsync(It.IsAny<Guid>(), CancellationToken.None)).Returns<Guid, CancellationToken>(
        //        (id, token) =>
        //        {
        //            Group group = GetGroup(id, studentIds);
        //            return Task.FromResult(group);
        //        });
        //    return repo;
        //}

        //private static IMapper GetMapper()
        //{
        //    var configuration = new MapperConfiguration(x => x.AddMaps(new List<Assembly>() { typeof(ApplicationServiceExtensions).Assembly }));
        //    return new Mapper(configuration);
        //}

        //private static IDomainFactory GetDomainFactory()
        //{
        //    var logger = new Mock<ILogger<Student>>().Object;
        //    var mock = new Mock<IDomainFactory>();
        //    mock.Setup(f => f.CreateStudent(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()))
        //        .Returns<Guid, string, string>((id, first, last) => new Student(id, first, last));
        //    return mock.Object;
        //}

       

        //private static GetGroupDetails.Handler GetHandlerWithEmptyRepoAndMapper()
        //{
        //    var repoStub = new Mock<IRepository<Group>>();
        //    repoStub.Setup(e => e.GetAsync(It.IsAny<Guid>(), CancellationToken.None)).Returns(() => Task.FromResult<Group>(null));

        //    var mapperStub = new Mock<IMapper>();
        //    mapperStub.Setup(e => e.Map<GroupModel>(It.IsAny<Group>())).Returns<Group>(null);

        //    return new GetGroupDetails.Handler(repoStub.Object, mapperStub.Object);
        //}
    }
}