using System;

namespace Azure.QueueStorage
{
    public class InsertOptions
    {
        public InsertOptions(TimeSpan? ttl, TimeSpan? visibilityDelay)
        {
            this.TimeToLive = ttl;
            this.VisibilityDelay = visibilityDelay;

        }
        public TimeSpan? TimeToLive { get; }
        public TimeSpan? VisibilityDelay { get; }
    }
}