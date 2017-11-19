using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace ticket_model
{
    public class QueueHelper
    {
        public QueueHelper(string connectionString, string queueName)
        {
            _storageAccount = CloudStorageAccount.Parse(connectionString);
            _queueClient = _storageAccount.CreateCloudQueueClient();
            _queue = _queueClient.GetQueueReference(queueName);
            _createQueueTask = _queue.CreateIfNotExistsAsync();
        }

        protected async Task WaitConfiguredAsync()
        {
            await _createQueueTask;
        }

        protected Task _createQueueTask;

        protected CloudStorageAccount _storageAccount;
        protected CloudQueueClient _queueClient;
        protected CloudQueue _queue;
    }
}
