using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using ticket_model;

namespace ticket_worker
{
    public class TicketHandler : QueueHelper
    {
        public TicketHandler(string connectionString, string queueName)
            : base(connectionString, queueName)
        {
        }

        public async Task HandleNextAsync()
        {
            await WaitConfiguredAsync();

            var msg = await _queue.GetMessageAsync(TimeSpan.FromSeconds(5), null, null);

            if (msg == null)
            {
                Console.WriteLine(".");
                Thread.Sleep(TimeSpan.FromMilliseconds(100));
                return;
            }

            if (msg.DequeueCount > 3)
            {
                await _queue.DeleteMessageAsync(msg);
                Console.WriteLine($"Removed {msg.AsString} since DequeueCount is too big");
            }

            Console.WriteLine($"Working on {msg.AsString}");

            var handlingDur = 2 + _rand.Next() % 5;
            for (int i = 0; i < handlingDur; ++i)
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
                Console.Write(".");
            }
            Console.WriteLine("");

            var ticket = JsonConvert.DeserializeObject<Ticket>(msg.AsString);
            if (ticket.Price % 8 == 0)
                throw new Exception($"Bad value {ticket.Price}");
            
            Console.WriteLine($"Finished with {msg.AsString}");
            await _queue.DeleteMessageAsync(msg);
        }

        private Random _rand = new Random(DateTime.Now.Millisecond);
    }
}
