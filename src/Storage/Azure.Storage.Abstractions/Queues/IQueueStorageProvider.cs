﻿using System.Threading;
using System.Threading.Tasks;

namespace Azure.Storage.Abstractions.Queues
{
    public interface IQueueStorageProvider
    {
        Task<QueueInsertResult> InsertAsync<T>(T payload, string queueName, InsertOptions options, CancellationToken ct);
    }
}