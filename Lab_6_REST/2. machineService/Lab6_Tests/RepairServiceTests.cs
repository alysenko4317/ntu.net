
using Moq;
using Xunit;
using System;

using Lab6.Models;
using Lab6.Services.Interfaces;
using Lab6.Repositories.Interfaces;

namespace Lab6_Tests
{
    public class RepairServiceTests
    {
        [Fact]
        public void WorkSuccessTest()
        {
            var serviceMock = new Mock<IRepairService>();
            var mockCars = new Mock<IBaseRepository<Car>>();
            var mockWorkers = new Mock<IBaseRepository<Worker>>();
            var mockDocs = new Mock<IBaseRepository<Document>>();
            var car = CreateCar(Guid.NewGuid());
            var worker = CreateWorker(Guid.NewGuid());
            var doc = CreateDoc(Guid.NewGuid(), worker.Id, car.Id);

            mockCars.Setup(x => x.Create(car)).Returns(car);
            mockDocs.Setup(x => x.Create(doc)).Returns(doc);
            mockWorkers.Setup(x => x.Create(worker)).Returns(worker);

            serviceMock.Object.Work();
            serviceMock.Verify(x => x.Work());
        }

        private Car CreateCar(Guid carId)
        {
            return new Car() {
                Id = carId,
                Name = "car",
                Number = "123"
            };
        }

        private Worker CreateWorker(Guid workerId)
        {
            return new Worker() {
                Id = workerId,
                Name = "worker",
                Position = "manager",
                Telephone = "89165555555"
            };
        }
        private Document CreateDoc(Guid docId, Guid workerId, Guid carId)
        {
            return new Document {
                Id = docId,
                CarId = carId,
                WorkerId = workerId
            };
        }
    }
}
