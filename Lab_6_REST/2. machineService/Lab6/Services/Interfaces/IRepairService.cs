
using System;

namespace Lab6.Services.Interfaces
{
    public interface IRepairService
    {
        public void Work();
        public void Work(Guid workerId, string carNumber, string carRegistrationNumber);
    }
}
