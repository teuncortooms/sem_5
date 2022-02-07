using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Domain.Entities
{
    public class Student : EntityBase
    {
        private readonly HashSet<Group> groups = new();

        public string FirstName { get; }
        public string LastName { get; }
        public string FullName { get { return $"{FirstName} {LastName}"; } }
        public string Email { get; }
        public IReadOnlyCollection<Group> Groups { get; }
        public Group CurrentGroup { get; private set; }

        public Student(Guid id, string firstName, string lastName, string email)
            : this(firstName, lastName, email)
        {
            if (id == default)
            {
                throw new ArgumentException($"'{nameof(id)}' cannot be empty.", nameof(id));
            }
            this.Id = id;
        }

        public Student(string firstName, string lastName, string email)
        {
            if (string.IsNullOrEmpty(firstName))
                throw new ArgumentException("Value cannot be null or empty.", nameof(firstName));
            if (string.IsNullOrEmpty(lastName))
                throw new ArgumentException("Value cannot be null or empty.", nameof(lastName));
            if (string.IsNullOrEmpty(email))
                throw new ArgumentException("Value cannot be null or empty.", nameof(email));

            this.Id = Guid.NewGuid();
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.groups = new HashSet<Group>();
            this.Groups = this.groups;
        }

        // copy constructor
        public Student(Student original)
            : this(original.Id, original.FirstName, original.LastName, original.Email)
        {
            this.AddGroups(original.Groups);
        }

        public void AddGroups(IEnumerable<Group> groups)
        {
            foreach (var group in groups)
            {
                AddGroup(group);
            }
        }

        public void AddGroup(Group group)
        {
            if (group == null) throw new ArgumentNullException(nameof(group));

            // TODO: validate group dates

            this.groups.Add(group);

            if (this.CurrentGroup == null)
            {
                UpdateCurrentGroup(group);
            }
        }

        public void RemoveGroups(IEnumerable<Group> groups)
        {
            foreach (var group in groups)
            {
                RemoveGroup(group.Id);
            }
        }

        public bool RemoveGroup(Guid groupId)
        {
            if (groupId == default) return false;

            int result = this.groups.RemoveWhere(g => g.Id == groupId);

            if (this.CurrentGroup != null && this.CurrentGroup.Id == groupId)
            {
                this.CurrentGroup = null;
            }

            return result > 0;
        }

        public void UpdateCurrentGroup(Group group)
        {
            if (group.StartDate < DateTime.Now && group.EndDate > DateTime.Now)
            {
                this.CurrentGroup = group;

                if (!this.Groups.Select(g => g.Id).Contains(group.Id))
                {
                    this.AddGroup(group);
                }
            }
        }

        public void UpdateCurrentGroup()
        {
            var candidates = new List<Group>();
            var today = DateTime.Now;
            foreach (var group in this.groups)
            {
                if (group.StartDate < today && group.EndDate > today)
                {
                    candidates.Add(group);
                }
            }
            if (candidates.Count == 0) this.CurrentGroup = null;
            else
            {
                if (candidates.Count > 1)
                {
                    // NB: cannot inject logger due to EF
                    Console.WriteLine($"Student {this.Id} should not have more than 1 current group candidate.");
                }
                this.CurrentGroup = candidates.OrderBy(g => g.StartDate).LastOrDefault();
            }
        }
    }
}
