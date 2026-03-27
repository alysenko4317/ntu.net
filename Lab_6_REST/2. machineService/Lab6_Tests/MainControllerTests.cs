
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using Lab6.Models;
using Lab6.Controllers;
using Lab6.Repositories.Interfaces;
using Lab6.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;  // for JsonResult
using Newtonsoft.Json;
using System.Reflection.Metadata;
using Document = Lab6.Models.Document;

namespace Lab6_Tests
{
    public class MainControllerTests
    {
        [Fact]
        public void GetAllDocuments()
        {
            var mockDocs = new Mock<IBaseRepository<Document>>();
            var mockService = new Mock<IRepairService>();
            var document = GetDoc();
            // setup to return the list that contains the document
            mockDocs.Setup(x => x.GetAll()).Returns(new List<Document> { document });
            MainController controller = new MainController(mockService.Object, mockDocs.Object);
            JsonResult result = controller.Get() as JsonResult;
            Assert.Equal(new List<Document> { document }, result?.Value);
        }

        [Fact]
        public void GetDocumentById()
        {
            var mockDocs = new Mock<IBaseRepository<Document>>();
            var mockService = new Mock<IRepairService>();
            var document = GetDoc();
            // setup to return the document when Get is called with the ID
            mockDocs.Setup(x => x.Get(document.Id)).Returns(document);
            MainController controller = new MainController(mockService.Object, mockDocs.Object);
            JsonResult result = new JsonResult(controller.GetById(document.Id).Value);
            Assert.Equal(document, result?.Value);
        }

        [Fact]
        public void GetDocumentById_NotFound()
        {
            var mockDocs = new Mock<IBaseRepository<Document>>();
            var mockService = new Mock<IRepairService>();
            mockDocs.Setup(x => x.Get(It.IsAny<Guid>())).Returns((Document)null); // assuming no documents are found
            MainController controller = new MainController(mockService.Object, mockDocs.Object);
            var result = controller.GetById(Guid.NewGuid()).Result as NotFoundResult;
            Assert.NotNull(result);  // ensuring that the result is a NotFoundResult
        }

        [Fact]
        public void CreateDocument()
        {
            // setup test data for repair request
            var repairRequest = new RepairRequest() {
                WorkerId = Guid.NewGuid(),
                CarName = "Skoda Rapid",
                CarRegistrationNumber = "AX5060AX"
            };

            var mockDocs = new Mock<IBaseRepository<Document>>();
            var mockService = new Mock<IRepairService>();

            // setup mock service to ensure the correct parameters are passed to Work() method
            mockService.Setup(service => service.Work(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()))
                       .Callback<Guid, string, string>((wId, carName, carNumber) => {
                           // check actually passed values
                           Assert.Equal(wId, repairRequest.WorkerId);
                           Assert.Equal(carName, repairRequest.CarName);
                           Assert.Equal(carNumber, repairRequest.CarRegistrationNumber);
                       });

            // creating the controller and invoking the Post method
            MainController controller = new MainController(mockService.Object, mockDocs.Object);
            JsonResult result = controller.Post(repairRequest) as JsonResult;
            Assert.Equal("Work was successfully done", result?.Value);
        }
        
        [Fact]
        public void ModifyDocument()
        {
            var originalDocument = GetDoc();
            var updatedDocument = GetDoc();
            updatedDocument.Id = originalDocument.Id; // assuming other properties are updated

            var mockDocs = new Mock<IBaseRepository<Document>>();
            var mockService = new Mock<IRepairService>();

            // setup to return the original document when Get is called with the ID
            mockDocs.Setup(x => x.Get(originalDocument.Id)).Returns(originalDocument);

            // setup to capture the updated document and verify it
            mockDocs.Setup(x => x.Update(It.IsAny<Document>()))
                    .Callback<Document>(d => {
                        // to compare objects by values convert them to JSON string and than compare
                        Assert.Equal(JsonConvert.SerializeObject(updatedDocument),
                                     JsonConvert.SerializeObject(d));
                    })
                    .Returns(updatedDocument); // return the updated document

            // creating the controller and invoking the Put method
            MainController controller = new MainController(mockService.Object, mockDocs.Object);
            JsonResult result = controller.Put(updatedDocument);
            Assert.Equal($"Update successful {originalDocument.Id}", result?.Value);
        }
        
        [Fact]
        public void DeleteDocument()
        {
            var mockDocs = new Mock<IBaseRepository<Document>>();
            var mockService = new Mock<IRepairService>();
            var document = GetDoc();

            // setup to return the document when Get is called with the ID
            mockDocs.Setup(x => x.Get(document.Id)).Returns(document);

            // setup to ensure that correct document ID was passed to the Delete method
            mockDocs.Setup(x => x.Delete(It.IsAny<Guid>()))
                    .Callback<Guid>(id => {
                        Assert.Equal(id, document.Id);
                    });

            // creating the controller and invoking the Delete method
            MainController controller = new MainController(mockService.Object, mockDocs.Object);
            JsonResult result = controller.Delete(document.Id) as JsonResult;
            Assert.Equal("Delete successful", result?.Value);
        }

        public Document GetDoc()
        {
            var carId = Guid.NewGuid();
            var workerId = Guid.NewGuid();
            var mockCars = new Mock<IBaseRepository<Car>>();
            var mockWorkers = new Mock<IBaseRepository<Worker>>();

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
