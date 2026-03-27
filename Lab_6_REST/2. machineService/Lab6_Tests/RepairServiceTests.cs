
using Moq;
using Xunit;
using System;

using Lab6.Models;
using Lab6.Services.Interfaces;
using Lab6.Repositories.Interfaces;
using Lab6.Services.Implementations;
using System.Reflection.Metadata;
using Document = Lab6.Models.Document;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Lab6_Tests
{
    public class RepairServiceTests
    {
        [Fact]
        public void WorkSuccessTest()
        {
            var (mockCars, mockWorkers, mockDocs) = SetupMocks();
            IRepairService service
                = new RepairService(mockDocs.Object, mockCars.Object, mockWorkers.Object);
            service.Work();  // test the logic
        }

        [Fact]
        public void WorkSuccessTest_withParameters()
        {
            var (mockCars, mockWorkers, mockDocs) = SetupMocks();
            // additional setup specific to this test
            mockCars.Setup(x => x.GetAll()).Returns(new List<Car> { CreateCar() });
            IRepairService service
                = new RepairService(mockDocs.Object, mockCars.Object, mockWorkers.Object);
            service.Work(CreateWorker().Id, CreateCar().Name, CreateCar().Number);
        }

        private (Mock<IBaseRepository<Car>>,
                 Mock<IBaseRepository<Worker>>,
                 Mock<IBaseRepository<Document>>) SetupMocks()
        {
            var mockCars = new Mock<IBaseRepository<Car>>();
            var mockWorkers = new Mock<IBaseRepository<Worker>>();
            var mockDocs = new Mock<IBaseRepository<Document>>();

            var car = CreateCar();
            var worker = CreateWorker();
            var doc = CreateDoc(worker.Id, car.Id);

            mockCars.Setup(x => x.Get(It.IsAny<Guid>())).Returns(car);
            mockWorkers.Setup(x => x.Get(It.IsAny<Guid>())).Returns(worker);
            mockCars.Setup(x => x.Create(It.IsAny<Car>())).Returns((Car c) => c);
            mockWorkers.Setup(x => x.Create(It.IsAny<Worker>())).Returns((Worker w) => w);
            mockDocs.Setup(x => x.Create(It.Is<Document>(d => d.WorkerId == worker.Id && d.CarId == car.Id))).Returns(doc);

            return (mockCars, mockWorkers, mockDocs);
        }

        private Car CreateCar() {
            return new Car() {
                Id = Guid.NewGuid(),
                Name = "Skoda Rapid",
                Number = "AX1234AX"
            };
        }

        private Worker CreateWorker() {
            return new Worker() {
                Id = Guid.NewGuid(),
                Name = "worker",
                Position = "manager",
                Telephone = "89165555555"
            };
        }

        private Document CreateDoc(Guid workerId, Guid carId) {
            return new Document {
                Id = Guid.NewGuid(),
                CarId = carId,
                WorkerId = workerId
            };
        }
    }
}
