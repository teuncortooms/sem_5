using Core.Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    public class DomainFactory : IDomainFactory
    {
        public Student CreateStudent(Guid id, string firstName, string lastName, string email)
        {
            return new Student(id, firstName, lastName, email);
        }

        public Student CreateStudent(string firstName, string lastName, string email)
        {
            return new Student(firstName, lastName, email);
        }

        public Student CopyStudent(Student student)
        {
            return new Student(student);
        }

        public Teacher CreateTeacher(string firstName, string lastName)
        {
            return new Teacher(firstName, lastName);
        }
    }
}
