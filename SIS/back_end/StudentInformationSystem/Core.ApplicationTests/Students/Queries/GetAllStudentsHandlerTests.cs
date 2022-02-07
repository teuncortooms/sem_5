//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Core.Application.Students.Queries.GetAllStudents;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Moq;
//using System.Threading;
//using Core.Application;
//using FluentAssertions;
//using Core.Application.Interfaces;
//using Core.Domain.Entities;

//namespace Core.Application.Students.Queries.GetAllStudents.Tests
//{
//    [TestClass()]
//    public class GetAllStudentsHandlerTests
//    {
//        private const string FIRSTNAME_PREFIX = "Firstname_";
//        private const string LASTNAME_PREFIX = "Lastname_";

//        [TestMethod()]
//        [DataRow(3)]
//        public void Handle_WithFunctionalRepo_ReturnsCorrectStudentsResponse(int dataSize)
//        {
//            //Arrange
//            GetAllStudentsHandler handler = GetDefaultHandler(dataSize);
//            List<StudentRequestModel> expected = GetExpectedResponse(dataSize);

//            // Act
//            var actual = handler.Handle(new GetAllStudentsQuery()).Result.ToList();

//            // Assert
//            actual.Should().BeEquivalentTo(expected);
//        }

//        [TestMethod()]
//        public void Handle_WithDisFunctionalRepo_ReturnsNull()
//        {
//            //Arrange
//            GetAllStudentsHandler handler = GetHandlerWithDisfunctionalRepo();

//            // Act
//            IEnumerable<StudentRequestModel> actual = handler.Handle(new GetAllStudentsQuery()).Result;

//            // Assert
//            actual.Should().BeNull();
//        }

//        private static GetAllStudentsHandler GetDefaultHandler(int dataSize)
//        {
//            IEnumerable<Student> students = GetStudents(dataSize);

//            var repoStub = new Mock<IRepository<Student>>();
//            repoStub.Setup(e => e.GetAllAsync(CancellationToken.None)).Returns(Task.FromResult(students));

//            return new GetAllStudentsHandler(repoStub.Object);
//        }

//        private static IEnumerable<Student> GetStudents(int dataSize)
//        {
//            var students = new List<Student>();

//            for (int i = 1; i <= dataSize; i++)
//            {
//                string firstname = FIRSTNAME_PREFIX + i;
//                string lastname = LASTNAME_PREFIX + i;
//                students.Add(new Student { Id = i, FirstName = firstname, LastName = lastname });
//            }

//            return students;
//        }

//        private static List<StudentRequestModel> GetExpectedResponse(int dataSize)
//        {
//            var response = new List<StudentRequestModel>();

//            for (int i = 1; i <= dataSize; i++)
//            {
//                response.Add(new StudentRequestModel { Id = i, FullName = $"{ FIRSTNAME_PREFIX }{ i } { LASTNAME_PREFIX }{ i }" });
//            }

//            return response;
//        }

//        private static GetAllStudentsHandler GetHandlerWithDisfunctionalRepo()
//        {
//            var repoStub = new Mock<IRepository<Student>>();
//            repoStub.Setup(e => e.GetAllAsync(CancellationToken.None)).Returns(() => Task.FromResult<IEnumerable<Student>>(null));

//            return new GetAllStudentsHandler(repoStub.Object);
//        }
//    }
//}