using Core.Domain.Entities;
using System;

namespace Core.Domain
{
    public interface IDomainFactory
    {
        Student CopyStudent(Student student);
        Student CreateStudent(Guid id, string firstName, string lastName, string email);
        Student CreateStudent(string firstName, string lastName, string email);
        Teacher CreateTeacher(string firstName, string lastName);
    }
}