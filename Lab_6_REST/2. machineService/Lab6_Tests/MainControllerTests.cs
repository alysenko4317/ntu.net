
using Moq;
using Xunit;
using System;
using System.Collections.Generic;

using Lab6.Models;
using Lab6.Controllers;
using Lab6.Repositories.Interfaces;
using Lab6.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;  // for JsonResult

namespace Lab6_Tests
{
    public class MainControllerTests
    {
        [Fact]
        public void GetDataMessage()
        {
            var mockDocs = new Mock<IBaseRepository<Document>>();
            var mockService = new Mock<IRepairService>();
            var document = GetDoc();
            mockDocs.Setup(x => x.GetAll()).Returns(new List<Document> { document });

            MainController controller = new MainController(mockService.Object, mockDocs.Object);  // Arrange
            JsonResult result = controller.Get() as JsonResult;  // Act
            Assert.Equal(new List<Document> { document }, result?.Value);  // Assert
        }

        [Fact]
        public void GetNotNull()
        {
            var mockDocs = new Mock<IBaseRepository<Document>>();
            var mockService = new Mock<IRepairService>();
            mockDocs.Setup(x => x.Create(GetDoc())).Returns(GetDoc());

            MainController controller = new MainController(mockService.Object, mockDocs.Object);  // Arrange
            JsonResult result = controller.Get() as JsonResult;  // Act
            Assert.NotNull(result);  // Assert
        }

        [Fact]
        public void PostDataMessage()
        {
            var mockDocs = new Mock<IBaseRepository<Document>>();
            var mockService = new Mock<IRepairService>();
            mockDocs.Setup(x => x.Create(GetDoc())).Returns(GetDoc());

            MainController controller = new MainController(mockService.Object, mockDocs.Object);  // Arrange
            JsonResult result = controller.Post() as JsonResult;  // Act
            Assert.Equal("Work was successfully done", result?.Value);  // Assert
        }

        [Fact]
        public void UpdateDataMessage()
        {
            var mockDocs = new Mock<IBaseRepository<Document>>();
            var mockService = new Mock<IRepairService>();
            var document = GetDoc();

            mockDocs.Setup(x => x.Get(document.Id)).Returns(document);
            mockDocs.Setup(x => x.Update(document)).Returns(document);

            MainController controller = new MainController(mockService.Object, mockDocs.Object);  // Arrange
            JsonResult result = controller.Put(document) as JsonResult;  // Act
            Assert.Equal($"Update successful {document.Id}", result?.Value);  // Assert
        }

        [Fact]
        public void DeleteDataMessage()
        {
            var mockDocs = new Mock<IBaseRepository<Document>>();
            var mockService = new Mock<IRepairService>();
            var doc = GetDoc();

            mockDocs.Setup(x => x.Get(doc.Id)).Returns(doc);
            mockDocs.Setup(x => x.Delete(doc.Id));

            MainController controller = new MainController(mockService.Object, mockDocs.Object);  // Arrange
            JsonResult result = controller.Delete(doc.Id) as JsonResult;  // Act
            Assert.Equal("Delete successful", result?.Value);  // Assert
        }

        public Document GetDoc()
        {
            var mockCars = new Mock<IBaseRepository<Car>>();
            var mockWorkers = new Mock<IBaseRepository<Worker>>();

            var carId = Guid.NewGuid();
            var workerId = Guid.NewGuid();
            mockCars.Setup(x => x.Create(new Car() {
                Id = carId,
                Name = "car",
                Number = "123"
            }));

            mockWorkers.Setup(x => x.Create(new Worker() {
                Id = workerId,
                Name = "worker",
                Position = "manager",
                Telephone = "89165555555"
            }));

            return new Document {
                Id = Guid.NewGuid(),
                CarId = carId,
                WorkerId = workerId
            };
        }
    }
}
