using Core.Domain.Entities;
using System;

namespace Core.Application.Groups.Models
{
    public class GroupModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Period { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public GroupModel(Group group)
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
        }
    }
}