using Core.Application.Extensions;
using Core.Application.Interfaces;
using Core.Application.Mocks;
using Core.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Core.Application.Commands
{
    public static class PerformSeeding
    {
        public class Command : IRequest
        {
            public int AmountStudents { get; set; }
            public int AmountGroups { get; set; }
            public int AmountGrades { get; set; }
            public Guid SecretKey { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly IRepository<Grade> _grades;
            private readonly IRepository<Student> _students;
            private readonly IConfiguration config;

            public Handler(IRepository<Grade> grades, IRepository<Student> students, IConfiguration config)
            {
                _grades = grades;
                _students = students;
                this.config = config ?? throw new ArgumentNullException(nameof(config));
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                if (request.SecretKey != new Guid(config["AdminKey"])) return Unit.Value;

                var developers = new List<Student>
                {
                    new Student(Guid.NewGuid(), "Test", "Test", "test@test.nl"),
                    new Student(Guid.NewGuid(), "Richard", "De Jong", "richard.dejong@student.fontys.nl"),
                    new Student(Guid.NewGuid(), "Ruud", "Beerkens", "r.beerkens@student.fontys.nl"),
                    new Student(Guid.NewGuid(), "Teun", "Cortooms", "teun.cortooms@student.fontys.nl"),
                    new Student(Guid.NewGuid(), "Teun", "Cortooms", "t.cortooms@fontys.nl"),
                    new Student(Guid.NewGuid(), "Teun", "Cortooms", "t.cortooms@fhict.nl")
                };

                var filteredDevs = developers.FindAll(developer => _students.Query().All(s => s.Email != developer.Email));

                var generator = new DataGenerator(filteredDevs, request.AmountStudents, request.AmountGroups, request.AmountGrades);

                generator.Students.ForEach(s => _students.Add(s));
                generator.Grades.ForEach(g => _grades.Add(g));
                await _students.CommitAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
