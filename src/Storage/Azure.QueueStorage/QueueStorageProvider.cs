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
using Azure.Storage.Abstractions;
using Azure.Storage.Abstractions.Queues;

namespace Azure.Storage.Queues
{
    public class QueueStorageProvider : StorageProvider, IQueueStorageProvider
    {
        private readonly IFormatter binaryFormatter;
        public QueueStorageProvider(StorageSettings settings, IFormatter binaryFormatter) : base(settings)
        {
            this.binaryFormatter = binaryFormatter;
        }

        private async Task<CloudQueue> GetOrCreateQueueAsync(string queueName, CloudStorageAccount storageAccount, CancellationToken ct)
        {
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue queue = queueClient.GetQueueReference(queueName);
            IsCancelled(ct);
            await queue.CreateIfNotExistsAsync();
            return queue;
        }
         
        public async Task<QueueInsertResult> InsertAsync<T>(T payload, string queueName, InsertOptions options, CancellationToken ct)
        {
            var storageAccount = this.GetCloudStorageAccount();
            IsCancelled(ct);
          
            CloudQueue queue = await GetOrCreateQueueAsync(queueName, storageAccount, ct);
            using (var memoryStream = new MemoryStream())
            {
                this.binaryFormatter.Serialize(memoryStream, payload);
                CloudQueueMessage message = new CloudQueueMessage(memoryStream.ToArray());
                await queue.AddMessageAsync(message, options.TimeToLive, options.VisibilityDelay, null, null, ct);
            } 

            return new QueueInsertResult(queue.Name, queue.Uri, queue.ApproximateMessageCount, queue.Metadata);
        } 

    }
}
