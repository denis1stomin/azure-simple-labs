using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using ticket_model;

namespace ticket_generator
{
    public class TicketSender : QueueHelper
    {
        public TicketSender(string connectionString, string queueName)
            : base(connectionString, queueName)
        {
        }

        public async Task SendAsync(Ticket ticket)
        {
            await WaitConfiguredAsync();

            var strMsg = JsonConvert.SerializeObject(ticket);
            var msg = new CloudQueueMessage(strMsg);
            await _queue.AddMessageAsync(msg);

            await _queue.FetchAttributesAsync();

            Console.WriteLine($"Sent {strMsg}, total = {_queue.ApproximateMessageCount}");
        }
    }
}
