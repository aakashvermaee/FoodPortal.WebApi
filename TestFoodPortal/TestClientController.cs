using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using Moq;
using FoodPortal.Data.Models;
using FoodPortal.Controllers;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace TestFoodPortal
{
    [TestClass]
    public class TestClientController
    {
        [TestMethod]
        public void PostClient_shouldAddClients()
        {
            var mockSet = new Mock<DbSet<Client>>();
            var mockContext = new Mock<FoodOrderingDbEntities>();

            mockContext.Setup(m => m.Clients).Returns(mockSet.Object);
            var service = new ClientsController(mockContext.Object);
            service.PostClient(getDemoClient());
            mockSet.Verify(m => m.Add(It.IsAny<Client>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void ClientLogin_shouldReturnClientId()
        {
            var data = new List<Client>
            {
                new Client { ClientId = "AAA", Password = "123" }
            };
            var queryData = data.AsQueryable();

            var mockSet = new Mock<DbSet<Client>>();
            mockSet.As<IQueryable<Client>>().Setup(m => m.Provider).Returns(queryData.Provider);
            mockSet.As<IQueryable<Client>>().Setup(m => m.Expression).Returns(queryData.Expression);
            mockSet.As<IQueryable<Client>>().Setup(m => m.ElementType).Returns(queryData.ElementType);
            mockSet.As<IQueryable<Client>>().Setup(m => m.GetEnumerator()).Returns(queryData.GetEnumerator());

            var mockContext = new Mock<FoodOrderingDbEntities>();

            mockContext.Setup(m => m.Clients).Returns(mockSet.Object);
            var service = new ClientsController(mockContext.Object);
            var result = service.ClientLogin(new Client { ClientId = "AAA", Password = "123" });

            Assert.AreEqual(data[0].ClientId, result.Content);

        }

        [TestMethod]
        public void UserExists_shouldCheckExistingUsers()
        {
            var data = new List<Client>
            {
                new Client { ClientId = "AAA", Password = "123" }
            };
            var queryData = data.AsQueryable();

            var mockSet = new Mock<DbSet<Client>>();
            mockSet.As<IQueryable<Client>>().Setup(m => m.Provider).Returns(queryData.Provider);
            mockSet.As<IQueryable<Client>>().Setup(m => m.Expression).Returns(queryData.Expression);
            mockSet.As<IQueryable<Client>>().Setup(m => m.ElementType).Returns(queryData.ElementType);
            mockSet.As<IQueryable<Client>>().Setup(m => m.GetEnumerator()).Returns(queryData.GetEnumerator());

            var mockContext = new Mock<FoodOrderingDbEntities>();

            mockContext.Setup(m => m.Clients).Returns(mockSet.Object);
            var service = new ClientsController(mockContext.Object);
            var result = service.UserExists(new Client { ClientId = "AAA", Password = "123" });

            Assert.AreEqual("Username Taken!", result.Content);

        }

        [TestMethod]
        public void EmailExists_shouldCheckExistingEmails()
        {
            var data = new List<Client>
            {
                new Client { ClientId = "AAA", Password = "123", Email = "abc@gmail.com" }
            };
            var queryData = data.AsQueryable();

            var mockSet = new Mock<DbSet<Client>>();
            mockSet.As<IQueryable<Client>>().Setup(m => m.Provider).Returns(queryData.Provider);
            mockSet.As<IQueryable<Client>>().Setup(m => m.Expression).Returns(queryData.Expression);
            mockSet.As<IQueryable<Client>>().Setup(m => m.ElementType).Returns(queryData.ElementType);
            mockSet.As<IQueryable<Client>>().Setup(m => m.GetEnumerator()).Returns(queryData.GetEnumerator());

            var mockContext = new Mock<FoodOrderingDbEntities>();

            mockContext.Setup(m => m.Clients).Returns(mockSet.Object);
            var service = new ClientsController(mockContext.Object);
            var result = service.EmailExists(new Client { ClientId = "AAA", Password = "123", Email = "aaa@gmail.com" });

            Assert.AreEqual("Email ID can be used", result.Content);

        }

        Client getDemoClient()
        {
            return new Client() { ClientId = "abc", Password = "1234", Name = "abc", Email = "abc@gmail", Address = "abc/1234", Contact = "98765432" };
        }
    }
}
