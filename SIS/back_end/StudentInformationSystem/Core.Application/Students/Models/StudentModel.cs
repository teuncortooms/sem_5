using Core.Application.Groups.Models;
using Core.Domain.Entities;
using System;

namespace Core.Application.Students.Models
{
    public class StudentModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public MinimalGroupModel CurrentGroup { get; set; }

        public StudentModel(Student student)
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
                CurrentGroup = new MinimalGroupModel(student.CurrentGroup);
            }
        }
    }
}