using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AsyncStreams
{
    class Program
    {
        //-------------------------------------------------------------------------------
        // Async Streams
        //-------------------------------------------------------------------------------

        public static async Task Main(string[] args)
        {
            Repository repo = new Repository();

            IAsyncEnumerable<string> data = repo.GetDataAsync();
            await foreach (var name in data)
                Console.WriteLine(name);
        }

        class Repository
        {
            string[] data = { "Tom", "Sam", "Kate", "Alice", "Bob" };

            public async IAsyncEnumerable<string> GetDataAsync()
            {
                for (int i = 0; i < data.Length; i++)
                {
                    Console.WriteLine($"Получаем {i + 1} элемент");
                    await Task.Delay(500);
                    yield return data[i];
                }
            }
        }
    }
}
