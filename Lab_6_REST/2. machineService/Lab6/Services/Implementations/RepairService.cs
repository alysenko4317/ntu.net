using System;
using System.Linq;
using Lab6.Models;
using Lab6.Repositories.Interfaces;
using Lab6.Services.Interfaces;

namespace Lab6.Services.Implementations
{
    public class RepairService : IRepairService
    {
        private IBaseRepository<Document> Documents { get; set; }
        private IBaseRepository<Car> Cars { get; set; }
        private IBaseRepository<Worker> Workers { get; set; }

        public RepairService(
            IBaseRepository<Document> documents,
            IBaseRepository<Car> cars,
            IBaseRepository<Worker> workers)
        {
            Documents = documents;
            Cars = cars;
            Workers = workers;
        }

        public void Work()
        {
            var rand = new Random();
            var carId = Guid.NewGuid();
            var workerId = Guid.NewGuid();

            Cars.Create(new Car {
                Id = carId,
                Name = String.Format($"Car{rand.Next()}"),
                Number = String.Format($"{rand.Next()}")
            });

            Workers.Create(new Worker {
                Id = workerId,
                Name = String.Format($"Worker{rand.Next()}"),
                Position = String.Format($"Position{rand.Next()}"),
                Telephone = String.Format(
                    $"8916{rand.Next()}{rand.Next()}{rand.Next()}" +
                    $"{rand.Next()}{rand.Next()}{rand.Next()}{rand.Next()}")
            });

            var car = Cars.Get(carId);
            var worker = Workers.Get(workerId);

            Documents.Create(new Document {
                CarId = car.Id,
                WorkerId = worker.Id,
                Car = car,
                Worker = worker
            });
        }

        public void Work(Guid workerId, string carName, string carRegistrationNumber)
        {
            var rand = new Random();

            // Перевіряємо, чи існує працівник
            var worker = Workers.Get(workerId);
            if (worker == null)
                throw new Exception("Worker not found");

            // Перевіряємо, чи існує автомобіль з заданим номером
            var car = Cars.GetAll().FirstOrDefault(c => c.Number == carRegistrationNumber);
            if (car == null) {
                // Якщо автомобіль не знайдено, створюємо новий
                car = new Car {
                    Id = Guid.NewGuid(),
                    Name = carName,
                    Number = carRegistrationNumber
                };
                Cars.Create(car);
            }

            // Створюємо документ про ремонт
            Documents.Create(new Document {
                CarId = car.Id,
                WorkerId = worker.Id,
                Car = car,
                Worker = worker
            });
        }
    }
}
