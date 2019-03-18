using System;
using System.Collections.Generic;

namespace Azure.Storage.Abstractions.Queues
{
    public class QueueInsertResult
    {
        public QueueInsertResult(string name,Uri uri, int? approximateQueueCount, System.Collections.Generic.IDictionary<string, string> metadata)
        {
            this.Name = name;
            this.Uri = uri;
            this.ApproximateMessageCount = approximateQueueCount;
            this.Metadata = metadata;
        }

        public string Name { get; set; }
        public Uri Uri { get; }
        public int? ApproximateMessageCount { get;  }
        public IDictionary<string, string> Metadata { get; }
    }
}