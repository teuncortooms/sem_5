using Core.Application.Groups.Models;
using Core.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Core.Application.Students.Models
{
    public class StudentDetailsModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public GroupModel CurrentGroup { get; set; }
        public ICollection<GroupModel> Groups { get; set; } = new List<GroupModel>();

        public StudentDetailsModel(Student student)
        {
            if (student == null)
            {
                throw new ArgumentNullException(nameof(student));
            }

            Id = student.Id;
            FirstName = student.FirstName;
            LastName = student.LastName;
            FullName = student.FullName;
            Email = student.Email;
            if (student.CurrentGroup != null)
            {
                CurrentGroup = new GroupModel(student.CurrentGroup);
            }

            foreach (var group in student.Groups)
            {
                Groups.Add(new GroupModel(group));
            }
        }
    }
}
