using Core.Application.Students.Models;
using Core.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Core.Application.Groups.Models
{
    public class GroupDetailsModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Period { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<MinimalStudentModel> Students { get; } = new List<MinimalStudentModel>();

        public GroupDetailsModel(Group group)
        {
            if (group == null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            Id = group.Id;
            Name = group.Name;
            Period = group.Period;
            StartDate = group.StartDate;
            EndDate = group.EndDate;

            foreach (var student in group.Students)
            {
                Students.Add(new MinimalStudentModel(student));
            }
        }

        public GroupDetailsModel(Guid id, string name, string period, DateTime start, DateTime end)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
            }

            if (string.IsNullOrEmpty(period))
            {
                throw new ArgumentException($"'{nameof(period)}' cannot be null or empty.", nameof(period));
            }

            Id = id;
            Name = name;
            Period = period;
            StartDate = start;
            EndDate = end;
        }
    }
}
