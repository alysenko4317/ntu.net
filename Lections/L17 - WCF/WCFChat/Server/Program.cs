
using System;
using System.ServiceModel;
using ChatLibrary;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // використання IDisposable
            using (var host = new ServiceHost(typeof(ChatService)))
            {
                host.Open();
                Console.WriteLine("Server is started...");
                Console.ReadLine();
            }
        }
    }
}
 