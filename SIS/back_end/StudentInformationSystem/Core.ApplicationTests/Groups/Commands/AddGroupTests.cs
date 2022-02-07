//using System;
//using Core.Application.Interfaces;
//using Core.Application.Mocks;
//using Core.Domain.Entities;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using static Core.Application.Groups.Commands.AddGroup;
//using Moq;


//namespace Core.ApplicationTests.Groups.Commands
//{
//    [TestClass()]
//    public class AddGroupTests
//    {
//        [TestMethod()]
//        public void AddGroup_ShouldWork()
//        {
//            var repo = new Mock<IRepository<Group>>().Object;
//            var handler = new Handler(repo);
//            var command = new Command()
//            {
//                Name = "Group0", Period = "Period0", StartDate = DateTime.Today.AddMonths(-3),
//                EndDate = DateTime.Today.AddMonths(3)
//            };

//            var response = handler.Handle(command);

//            // mockdb should have group



//        }
//    }
//}