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
    public class GroupTests
    {
        [TestMethod()]
        public void WhenOneStudentAddedToGroup_GroupHasStudent()
        {
            var group = GivenGroup().Build();
            var student = GivenStudent().WithFirstName("Teun").Build();

            group.AddStudents(new[] { student });

            group.Students.Should().Contain(student);
        }

        [TestMethod()]
        public void WhenMoreStudentsAddedToGroup_GroupHasStudents()
        {
            var group = GivenGroup().Build();
            var students = new List<Student>
            {
                GivenStudent().WithFirstName("Teun").Build(),
                GivenStudent().WithFirstName("Ruud").Build(),
            };

            group.AddStudents(students);

            group.Students.Should().Contain(students);
        }

        [TestMethod()]
        public void WhenStudentsAddedToGroup_GroupStillHasOriginalStudents()
        {
            var group = GivenGroup().Build();
            var studentsSet1 = new List<Student>
            {
                GivenStudent().WithFirstName("Teun").Build(),
                GivenStudent().WithFirstName("Ruud").Build(),
            };

            group.AddStudents(studentsSet1);

            var studentsSet2 = new List<Student>
            {
                GivenStudent().WithFirstName("Richard").Build(),
                GivenStudent().WithFirstName("Dennis").Build(),
            };

            group.AddStudents(studentsSet2);

            group.Students.Should().Contain(studentsSet1);
        }
    }
}