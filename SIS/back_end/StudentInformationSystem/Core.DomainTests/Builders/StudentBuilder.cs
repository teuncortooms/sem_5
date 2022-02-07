using System;
using System.Collections.Generic;
using Core.Domain.Entities;
using static Core.DomainTests.Builders.GroupBuilder;

namespace Core.DomainTests.Builders
{
    public class StudentBuilder
    {
        private readonly Guid id = Guid.NewGuid();
        private string firstName = "Elvis";
        private readonly string lastName = "Presley";
        private readonly string email = "no@email.nl";

        private List<Group> groups = new()
        {
            GivenGroup().WithPastDates().Build(), 
            GivenGroup().WithCurrentDates().Build()
        };

        public static StudentBuilder GivenStudent() => new();

        public StudentBuilder WithFirstName(string firstName)
        {
            this.firstName = firstName;
            return this;
        }

        public StudentBuilder WithGroups(List<Group> groups)
        {
            this.groups = groups;
            return this;
        }

        public Student Build()
        {
            var student = new Student(this.id, this.firstName, this.lastName, this.email);
            student.AddGroups(this.groups);
            return student;
        }
    }
}
