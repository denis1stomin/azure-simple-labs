using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace ticket_worker
{
    class Program
    {
        static IConfiguration Configuration;

        static void Main(string[] args)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var handler = new TicketHandler(
                Configuration["StorageAccConnectionString"], Configuration["StorageQueueName"]);

            while (true)
            {
                try
                {
                    handler.HandleNextAsync().Wait();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("========================================");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("========================================");
                }
            }
        }
    }
}
