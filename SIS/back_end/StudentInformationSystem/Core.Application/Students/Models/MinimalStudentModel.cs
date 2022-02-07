using Core.Domain.Entities;
using System;

namespace Core.Application.Students.Models
{
    public class MinimalStudentModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }

        public MinimalStudentModel(Guid id, string firstName, string lastName, string fullName)
        {
            Id = id;
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            FullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
        }

        public MinimalStudentModel(Student student)
        {
            if (student == null)
            {
                throw new ArgumentNullException(nameof(student));
            }

            Id = student.Id;
            FirstName = student.FirstName;
            LastName = student.LastName;
            FullName = student.FullName;
        }
    }
}