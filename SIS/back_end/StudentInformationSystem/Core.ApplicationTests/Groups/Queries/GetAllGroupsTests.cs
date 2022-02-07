using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domain.Entities;
using Moq;
using Core.Application.Interfaces;
using System.Threading;
using FluentAssertions;
using Core.Application.Groups.Models;
using AutoMapper;
using Core.Application.QueryUtil;
using Core.Application.Profiles;
using static Core.ApplicationTests.Builders.GroupRepoMocker;
using static Core.ApplicationTests.Builders.MapperBuilder;
using static Core.ApplicationTests.Builders.GetAllGroupsHandlerBuilder;


namespace Core.Application.Groups.Queries.Tests
{
    [TestClass()]
    public class GetAllGroupsTests
    {
        [TestMethod()]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(10)]
        [DataRow(100)]
        [DataRow(1000)]
        public async void Handler_ReturnsGroupWithCorrectProperties(int dataSize)
        {
            //Arrange
            var groupRepo = GroupRepoMock().WithGroups(dataSize).Build();
            var mapper = DefaultMapper().Build();
            var handler = DefaultHandler().WithGroupsRepo(groupRepo).Build();

            var groups = await groupRepo.GetAllAsync();
            List<GroupModel> expected = mapper.Map<List<GroupModel>>(groups);

            // Act
            var query = new GetAllGroups.Query() { Page = 1, PageSize = dataSize };
            var response = handler.Handle(query).Result;
            var actual = response.Results;

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [TestMethod()]
        public void Handler_WithEmptyRepo_ReturnsNull()
        {
            //Arrange
            var handler = DefaultHandler().Build();

            // Act
            var query = new GetAllGroups.Query();
            var pagedlist = handler.Handle(query).Result;
            var actual = pagedlist.Results;

            // Assert
            actual.Should().BeEmpty();
        }
    }
}