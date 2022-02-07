using Core.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Core.Application.Mocks
{
    public class MockDataContext : IMockDataContext
    {
        public List<Group> Groups { get; } = new();
        public List<Student> Students { get; } = new();
        public List<Grade> Grades { get; } = new();

        public List<T> GetEntities<T>() where T : EntityBase {
            if (typeof(T) == typeof(Group)) return Groups as List<T>;
            if (typeof(T) == typeof(Student)) return Students as List<T>;
            if (typeof(T) == typeof(Grade)) return Grades as List<T>;
            throw new ArgumentException("Type unknown");
        }

        public MockDataContext()
        {
            var generator = new DataGenerator(null, 200, 20, 200);
            Students = generator.Students;
            Grades = generator.Grades;
        }
    }
}
