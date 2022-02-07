using System;
using Core.Domain.Entities;

namespace Core.DomainTests.Builders
{
    public class GroupBuilder
    {
        private DateTime endDate = new(2022, 2, 28);
        private DateTime startDate = new(2021, 9, 1);
        private readonly string period = "TestPeriod";
        private readonly string name = "TestName";
        private readonly Guid id = Guid.NewGuid();

        public static GroupBuilder GivenGroup() => new();

        public GroupBuilder WithStartDate(DateTime startDate)
        {
            this.startDate = startDate;
            return this;
        }

        public GroupBuilder WithEndDate(DateTime endDate)
        {
            this.endDate = endDate;
            return this;
        }

        public GroupBuilder WithCurrentDates()
        {
            this.startDate = DateTime.Today.AddMonths(-3);
            this.endDate = DateTime.Today.AddMonths(3);
            return this;
        }

        public GroupBuilder WithPastDates()
        {
            this.startDate = DateTime.Today.AddMonths(-12);
            this.endDate = DateTime.Today.AddMonths(-6);
            return this;
        }

        public Group Build()
        {
            return new Group(this.id, this.name, this.period, this.startDate, this.endDate);
        }
    }
}
