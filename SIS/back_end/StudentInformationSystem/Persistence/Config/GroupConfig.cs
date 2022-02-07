using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace Persistence.Config
{
    internal class GroupConfig : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.HasKey(g => g.Id);
            builder.Property(g => g.Id).IsRequired(); // already present guid on Add could cause Update
            builder.Property(g => g.Name).IsRequired();
            builder.Property(g => g.Period).IsRequired();
            builder.Property(g => g.StartDate).IsRequired();
            builder.Property(g => g.EndDate).IsRequired();
        }
    }
}
