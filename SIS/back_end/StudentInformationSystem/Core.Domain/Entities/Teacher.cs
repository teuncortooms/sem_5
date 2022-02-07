using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Entities
{
    public class Teacher : EntityBase
    {        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return $"{FirstName} {LastName}"; } }
        public List<Group> Groups { get; }

        public Teacher(Guid id, string firstName, string lastName)
            : this(firstName, lastName)
        {
            if (id == default)
            {
                throw new ArgumentException($"'{nameof(id)}' cannot be empty.", nameof(id));
            }
            this.Id = id;
        }

        public Teacher(string firstName, string lastName)
        {
            if (string.IsNullOrEmpty(firstName))
            {
                throw new ArgumentException($"'{nameof(firstName)}' cannot be null or empty.", nameof(firstName));
            }

            if (string.IsNullOrEmpty(lastName))
            {
                throw new ArgumentException($"'{nameof(lastName)}' cannot be null or empty.", nameof(lastName));
            }

            this.Id = Guid.NewGuid();
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Groups = new List<Group>();
        }

        // copy constructor
        public Teacher(Teacher original)
            : this(original.Id, original.FirstName, original.LastName)
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

            this.Groups.Add(group);
        }
    }
}
