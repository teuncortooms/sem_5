using Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Core.Application.Mocks
{
    public class DataGenerator
    {
        private readonly string[] firstNames = {
            "Teun", "Richard", "Ruud", "Job", "Dennis", "Haran", "Adam", "Alex", "Aaron", "Ben", "Carl", "Dan", "David", "Edward", "Fred", "Frank", "George", "Hal", "Hank", "Ike", "John", "Jack", "Joe", "Larry", "Monte", "Matthew", "Mark", "Nathan", "Otto", "Paul", "Peter", "Roger", "Roger", "Steve", "Thomas", "Tim", "Ty", "Victor", "Walter"
        };
        private readonly string[] lastNames = {
            "Cortooms", "de Jong", "Beerkens", "Janssens", "Cools", "Moraal", "Anderson", "Ashwoon", "Aikin", "Bateman", "Bongard", "Bowers", "Boyd", "Cannon", "Cast", "Deitz", "Dewalt", "Ebner", "Frick", "Hancock", "Haworth", "Hesch", "Hoffman", "Kassing", "Knutson", "Lawless", "Lawicki", "Mccord", "McCormack", "Miller", "Myers", "Nugent", "Ortiz", "Orwig", "Ory", "Paiser", "Pak", "Pettigrew", "Quinn", "Quizoz", "Ramachandran", "Resnick", "Sagar", "Schickowski", "Schiebel", "Sellon", "Severson", "Shaffer", "Solberg", "Soloman", "Sonderling", "Soukup", "Soulis", "Stahl", "Sweeney", "Tandy", "Trebil", "Trusela", "Trussel", "Turco", "Uddin", "Uflan", "Ulrich", "Upson", "Vader", "Vail", "Valente", "Van Zandt", "Vanderpoel", "Ventotla", "Vogal", "Wagle", "Wagner", "Wakefield", "Weinstein", "Weiss", "Woo", "Yang", "Yates", "Yocum", "Zeaser", "Zeller", "Ziegler", "Bauer", "Baxster", "Casal", "Cataldi", "Caswell", "Celedon", "Chambers", "Chapman", "Christensen", "Darnell", "Davidson", "Davis", "DeLorenzo", "Dinkins", "Doran", "Dugelman", "Dugan", "Duffman", "Eastman", "Ferro", "Ferry", "Fletcher", "Fietzer", "Hylan", "Hydinger", "Illingsworth", "Ingram", "Irwin", "Jagtap", "Jenson", "Johnson", "Johnsen", "Jones", "Jurgenson", "Kalleg", "Kaskel", "Keller", "Leisinger", "LePage", "Lewis", "Linde", "Lulloff", "Maki", "Martin", "McGinnis", "Mills", "Moody", "Moore", "Napier", "Nelson", "Norquist", "Nuttle", "Olson", "Ostrander", "Reamer", "Reardon", "Reyes", "Rice", "Ripka", "Roberts", "Rogers", "Root", "Sandstrom", "Sawyer", "Schlicht", "Schmitt", "Schwager", "Schutz", "Schuster", "Tapia", "Thompson", "Tiernan", "Tisler"
        };
        private readonly string[] groupPrefixes =
        {
            "S", "T", "B", "M", "I"
        };
        private readonly int N_STUDENTS;
        private readonly int N_GROUPS;
        private readonly int N_GRADES;
        private int grades = 0;

        public List<Student> Students { get; } = new();
        public List<Group> Groups { get; } = new();
        public List<Grade> Grades { get; } = new();

        public DataGenerator(List<Student> students, int studentAmount, int groupAmount, int gradeAmount)
        {
            N_STUDENTS = studentAmount;
            N_GROUPS = groupAmount;
            N_GRADES = gradeAmount;
            Random r = new(1);
            InitStudents(r, students);
            InitGroups(r);
            UpdateCurrentGroups();
        }

        private void InitStudents(Random r, List<Student> students)
        {
            if (students != null) Students.AddRange(students);

            for (int i = 0; i < N_STUDENTS; i++)
            {
                string firstName = firstNames[r.Next(0, firstNames.Length)];
                string lastName = lastNames[r.Next(0, lastNames.Length)];
                string email = $"{firstName.ToLower()[0]}.{lastName.ToLower()}.{r.Next(100, 999)}@student.fontys.nl";
                Debug.Print(email);
                Students.Add(new Student(
                    Guid.NewGuid(),
                    firstName,
                    lastName,
                    email
                ));
            }
        }

        private void InitGroups(Random r)
        {
            DateTime defaultDate = new(2021, 9, 1);

            for (int i = 0; i < N_GROUPS; i++)
            {
                Guid id = Guid.NewGuid();
                string period = groupPrefixes[r.Next(groupPrefixes.Length)] + r.Next(1, 8);
                string name = period + "-0" + r.Next(1, 10);
                DateTime startDate = defaultDate.AddMonths(r.Next(-6, 1) * 6);
                DateTime endDate = startDate.AddMonths(6).AddDays(-1);

                Group group = new(id, name, period, startDate, endDate);

                AddStudentsAndGrades(r, group);

                this.Groups.Add(group);
            }
        }
        private void UpdateCurrentGroups()
        {
            foreach (Student s in this.Students)
            {
                s.UpdateCurrentGroup();
            }
        }

        private void AddStudentsAndGrades(Random r, Group group)
        {
            int nStudents = r.Next(20, 30);
            for (int j = 0; j < nStudents; j++)
            {
                int index = r.Next(this.Students.Count);
                Student student = this.Students[index];
                if (group.Students.Contains(student)) break;
                student.AddGroup(group);
                group.Students.Add(student);
                if (grades < N_GRADES)
                {
                    AddGrade(group, student, r);
                    grades++;
                }
            }
        }

        private void AddGrade(Group group, Student student, Random r)
        {
            Grades.Add(new Grade
            {
                Group = group,
                Student = student,
                Id = Guid.NewGuid(),
                Score = r.Next(0, 100)
            });
        }

        public List<Student> StudentsNoNavigation
        {
            get
            {
                List<Student> students = new();
                Students.ForEach(s => students.Add(
                    new Student(s.Id, s.FirstName, s.LastName, s.Email))
                );
                return students;
            }
        }


        public List<Group> GroupsNoNavigation
        {
            get
            {
                List<Group> groups = new();
                Groups.ForEach(g => groups.Add(
                    new Group(g.Id, g.Name, g.Period, g.StartDate, g.EndDate))
                );
                return groups;
            }
        }


        public List<Dictionary<string, object>> StudentGroupRelationships
        {
            get
            {
                List<Dictionary<string, object>> relationships = new();
                Groups.ForEach(g =>
                {
                    foreach (var s in g.Students)
                    {
                        relationships.Add(new Dictionary<string, object> {
                            { "StudentId", s.Id },
                            { "GroupId", g.Id }
                        });
                    }
                });
                return relationships;
            }
        }


        public List<Grade> GradesNoNavigation
        {
            get
            {
                List<Grade> grades = new();
                Grades.ForEach(g => grades.Add(
                    new Grade()
                    {
                        Id = g.Id,
                        GroupId = g.Group.Id,
                        Score = g.Score,
                        StudentId = g.Student.Id
                    }
                ));
                return grades;
            }
        }

    }
}
