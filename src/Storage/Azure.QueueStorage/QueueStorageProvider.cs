using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
 using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Azure.Storage;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

namespace Azure.Storage.Queues
{
    public class QueueStorageProvider : StorageProvider, IQueueStorageProvider
    {
        private readonly IFormatter binaryFormatter;
        public QueueStorageProvider(StorageSettings settings, IFormatter binaryFormatter) : base(settings)
        {
            this.binaryFormatter = binaryFormatter;
        }


        public async Task<QueueInsertResult> InsertAsync<T>(T payload, string queueName, InsertOptions options, CancellationToken ct)
        {

            var storageAccount = this.GetCloudStorageAccount();

            IsCancelled(ct);
            // Create the queue client.
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a queue.
            CloudQueue queue = queueClient.GetQueueReference(queueName);

            IsCancelled(ct);

            // Create the queue if it doesn't already exist.
            queue.CreateIfNotExists();

            using (var memoryStream = new MemoryStream())
            {
                this.binaryFormatter.Serialize(memoryStream, payload);
                CloudQueueMessage message = new CloudQueueMessage(memoryStream.ToArray());
                await queue.AddMessageAsync(message, options.TimeToLive, options.VisibilityDelay,null, null, ct);
            }

            return new QueueInsertResult();
        }

     

    }
}
