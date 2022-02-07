using Core.Application.Mocks;
using Core.Domain;
using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace Persistence.Config
{
    internal class StudentConfig : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id).IsRequired(); // already present guid on Add could cause Update
            builder.Property(s => s.FirstName).IsRequired();
            builder.Property(s => s.LastName).IsRequired();
            builder.Property(s => s.Email).IsRequired();
            builder.HasIndex(s => s.Email).IsUnique();

            builder.Ignore(g => g.FullName);

            builder.HasOne(s => s.CurrentGroup).WithMany();

            builder.HasMany(s => s.Groups).WithMany(g => g.Students)
                .UsingEntity<Dictionary<string, object>>(
                    "Student_Group",
                    j => j
                        .HasOne<Group>()
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .HasConstraintName("FK_StudentGroup_Group_GroupId")
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j
                        .HasOne<Student>()
                        .WithMany()
                        .HasForeignKey("StudentId")
                        .HasConstraintName("FK_StudentGroup_Student_StudentId")
            );
        }
    }
}
