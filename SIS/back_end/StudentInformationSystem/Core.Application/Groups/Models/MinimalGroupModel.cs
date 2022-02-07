using Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Groups.Models
{
    public class MinimalGroupModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public MinimalGroupModel(Group group)
        {
            if (group == null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            this.Id = group.Id;
            this.Name = group.Name;
        }
    }
}
