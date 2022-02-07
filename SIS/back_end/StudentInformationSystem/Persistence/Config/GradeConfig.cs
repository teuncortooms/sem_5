using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Config
{
    internal class GradeConfig : IEntityTypeConfiguration<Grade>
    {
        public void Configure(EntityTypeBuilder<Grade> builder)
        {
            builder.Navigation(g => g.Group).IsRequired();
            builder.Navigation(s => s.Student).IsRequired();
            builder.HasOne(e => e.Student)
                .WithMany();
            builder.HasOne(e => e.Group)
                .WithMany();
        }
    }
}
