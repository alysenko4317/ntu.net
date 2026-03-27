using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait
{
    class Program
    {
        // --------------------------------------------------------------------------
        // async/await - thread id change
        // --------------------------------------------------------------------------

        class Service
        {
            internal async Task InitializeAsync()
            {
                Console.WriteLine(
                    $"begin: thread_id={Thread.CurrentThread.ManagedThreadId}");

                await Task.Delay(5 * 1000);

                Console.WriteLine(
                    $"end: thread_id={Thread.CurrentThread.ManagedThreadId}");
            }

            public static async Task<Service> CreateAsync()
            {
                var service = new Service();
                await service.InitializeAsync();
                return service;
            }
        }

        public static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        // main can be async from C# 7.1
        public static async Task MainAsync(string[] args)
        {
            var service = await Service.CreateAsync();
            Console.WriteLine($"Service {service} is constructed!, thread_id={Thread.CurrentThread.ManagedThreadId}");
        }
    }
}
