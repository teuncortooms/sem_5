//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System.Threading.Tasks;
//using Moq;
//using System.Threading;
//using FluentAssertions;
//using Core.Application.Interfaces;
//using Core.Domain.Entities;

//namespace Core.Application.Students.Queries.GetStudentDetails.Tests
//{
//    [TestClass()]
//    public class GetStudentDetailsHandlerTests
//    {
//        private const string FIRSTNAME_PREFIX = "Firstname_";
//        private const string LASTNAME_PREFIX = "Lastname_";

//        [TestMethod()]
//        [DataRow(0)]
//        [DataRow(1)]
//        [DataRow(2)]
//        public void Handle_WithCorrectID_ReturnsCorrectStudentResponse(int id)
//        {
//            //Arrange
//            GetStudentDetailsHandler handler = GetDefaultHandler();
//            StudentDetailsRequestModel expected = GetExpectedResponse(id);

//            // Act
//            var actual = handler.Handle(new GetStudentDetailsQuery(id)).Result;

//            // Assert
//            actual.Should().BeEquivalentTo(expected);
//        }

//        [TestMethod()]
//        [DataRow(0)]
//        public void Handle_WithUnknownID_ReturnsNull(int id)
//        {
//            //Arrange
//            GetStudentDetailsHandler handler = GetHandlerWithEmptyRepo();

//            // Act
//            var actual = handler.Handle(new GetStudentDetailsQuery(id)).Result;

//            // Assert
//            actual.Should().BeNull();
//        }

//        private static GetStudentDetailsHandler GetDefaultHandler()
//        {
//            var repoStub = new Mock<IRepository<Student>>();
//            repoStub.Setup(e => e.GetAsync(It.IsAny<int>(), CancellationToken.None)).Returns<int, CancellationToken>((id, token) =>
//            {
//                Student student = GetStudent(id);
//                return Task.FromResult(student);
//            });

//            return new GetStudentDetailsHandler(repoStub.Object);
//        }

//        private static Student GetStudent(int id)
//        {
//            string firstname = FIRSTNAME_PREFIX + id;
//            string lastname = LASTNAME_PREFIX + id;
//            return new Student { Id = id, FirstName = firstname, LastName = lastname };
//        }

//        private static StudentDetailsRequestModel GetExpectedResponse(int id)
//        {
//            return new StudentDetailsRequestModel { Id = id, FirstName = FIRSTNAME_PREFIX + id, LastName = LASTNAME_PREFIX + id };
//        }

//        private static GetStudentDetailsHandler GetHandlerWithEmptyRepo()
//        {
//            var repoStub = new Mock<IRepository<Student>>();

//            repoStub.Setup(e => e.GetAsync(It.IsAny<int>(), CancellationToken.None)).Returns(() => Task.FromResult<Student>(null));

//            return new GetStudentDetailsHandler(repoStub.Object);
//        }
//    }
//}