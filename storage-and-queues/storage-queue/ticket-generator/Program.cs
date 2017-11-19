using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using ticket_model;

namespace ticket_generator
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

            int cnt = 0;
            var sender = new TicketSender(
                Configuration["StorageAccConnectionString"], Configuration["StorageQueueName"]);

            while (true)
            {
                var strInput = Console.ReadLine();
                
                if (string.Equals(strInput, "exit", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Bye..");
                    break;
                }
                
                if (int.TryParse(strInput, out var price))
                {
                    var ticket = new Ticket
                    {
                        Price = price,
                        Details = $"Number {++cnt}",
                    };

                    sender.SendAsync(ticket).Wait();
                }
                else
                {
                    Console.WriteLine("Should be integer value");
                }
            }
        }
    }
}
