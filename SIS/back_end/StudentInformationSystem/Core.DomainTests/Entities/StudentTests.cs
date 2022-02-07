using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using static Core.DomainTests.Builders.StudentBuilder;
using static Core.DomainTests.Builders.GroupBuilder;

namespace Core.Domain.Entities.Tests
{
    [TestClass()]
    public class StudentTests
    {
        [TestMethod()]
        public void WhenOneGroupAddedToStudent_StudentHasGroup()
        {
            var student = GivenStudent().Build();
            var group = GivenGroup().Build();

            student.AddGroup(group);

            student.Groups.Should().Contain(group);
        }

        [TestMethod()]
        public void WhenMoreGroupsAddedToStudent_StudentHasGroups()
        {
            var student = GivenStudent().Build();

            var groups = new List<Group>
            {
                GivenGroup().Build(),
                GivenGroup().Build(),
            };

            student.AddGroups(groups);

            student.Groups.Should().Contain(groups);
        }

        [TestMethod()]
        public void WhenGroupsAddedToStudent_StudentStillHasOriginalGroups()
        {
            var student = GivenStudent().Build();
            var groupsSet1 = new List<Group>
            {
                GivenGroup().Build(),
                GivenGroup().Build()
            };

            student.AddGroups(groupsSet1);

            var groupsSet2 = new List<Group>
            {
                GivenGroup().Build(),
                GivenGroup().Build()
            };

            student.AddGroups(groupsSet2);

            student.Groups.Should().Contain(groupsSet1);
        }

        [TestMethod()]
        public void WhenOneGroupRemovedFromStudent_StudentHasNotGroup()
        {
            var student = GivenStudent().Build();
            var group = GivenGroup().Build();

            student.AddGroup(group);
            student.RemoveGroup(group.Id);

            student.Groups.Should().NotContain(group);
        }


        [TestMethod()]
        public void WhenMoreGroupsRemovedFromStudent_StudentHasNotThoseGroups()
        {
            var student = GivenStudent().Build();
            var groups = new List<Group>
            {
                GivenGroup().Build(),
                GivenGroup().Build()
            };

            student.AddGroups(groups);
            student.RemoveGroups(groups);

            student.Groups.Should().NotContain(groups);
        }

        [TestMethod()]
        public void WhenGroupsRemovedFromStudent_StudentStillHasOtherGroups()
        {
            var student = GivenStudent().Build();
            var groupsSet1 = new List<Group>
            {
                GivenGroup().Build(),
                GivenGroup().Build()
            };

            student.AddGroups(groupsSet1);

            var groupsSet2 = new List<Group>
            {
                GivenGroup().Build(),
                GivenGroup().Build()
            };

            student.AddGroups(groupsSet2);
            student.RemoveGroups(groupsSet2);

            student.Groups.Should().Contain(groupsSet1);
        }

        [TestMethod()]
        public void WhenCurrentGroupUpdatedToNewGroup_StudentHasGroup()
        {
            var student = GivenStudent().Build();
            var newGroup = GivenGroup().WithCurrentDates().Build();

            student.UpdateCurrentGroup(newGroup);

            student.Groups.Should().Contain(newGroup);
        }

        [TestMethod()]
        public void GivenInvalidCurrentGroupCandidate_WhenUserUpdatesCurrentGroupToCandidate_CurrentGroupIsNotUpdated()
        {
            var student = GivenStudent().Build();
            var original = student.CurrentGroup;

            var newGroup = GivenGroup().WithPastDates().Build();
            student.UpdateCurrentGroup(newGroup);

            student.CurrentGroup.Should().BeSameAs(original).And.NotBeSameAs(newGroup);
        }

        [TestMethod()]
        public void WhenUserUpdatesCurrentGroupWithoutArgument_CurrentGroupEqualsMostCurrentGroup()
        {
            var olderGroups = new List<Group>
            {
                GivenGroup().WithPastDates().Build(),
                GivenGroup().WithPastDates().Build()
            };
            var student = GivenStudent().WithGroups(olderGroups).Build();
            var mostCurrentGroup = GivenGroup().WithCurrentDates().Build();
            student.AddGroup(mostCurrentGroup);

            student.UpdateCurrentGroup();

            student.CurrentGroup.Should().BeSameAs(mostCurrentGroup);
        }
    }
}