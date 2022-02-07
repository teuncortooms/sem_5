using System;
using System.Collections.Generic;

namespace Core.Domain.Entities
{
    public class Group : EntityBase
    {
        public string Name { get; }
        public string Period { get; }
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }
        public ICollection<Student> Students { get; }

        public Group(Guid id, string name, string period, DateTime startDate, DateTime endDate)
            : this(name, period, startDate, endDate)
        {
            if (id == default)
            {
                throw new ArgumentException($"'{nameof(id)}' cannot be empty.", nameof(id));
            }

            this.Id = id;
        }

        public Group(string name, string period, DateTime startDate, DateTime endDate)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
            }

            if (string.IsNullOrEmpty(period))
            {
                throw new ArgumentException($"'{nameof(period)}' cannot be null or empty.", nameof(period));
            }

            if (startDate == default)
            {
                throw new ArgumentException($"'{nameof(startDate)}' cannot be empty.", nameof(startDate));
            }

            if (endDate == default)
            {
                throw new ArgumentException($"'{nameof(endDate)}' cannot be empty.", nameof(endDate));
            }

            Id = Guid.NewGuid();
            Name = name;
            Period = period;
            StartDate = startDate;
            EndDate = endDate;
            this.Students = new List<Student>();
        }

        // copy constructor
        public Group(Group original)
            : this(original.Id, original.Name, original.Period, original.StartDate, original.EndDate)
        {
            AddStudents(original.Students);
        }

        public void AddStudents(IEnumerable<Student> students)
        {
            foreach (var student in students)
            {
                Students.Add(student);
            }
        }
    }
}
 