using System.Collections.Generic;

namespace Azure.Storage
{
    public class ListResult<T>
    {
        public ListResult(IEnumerable<T> cloudItems)
        {
            this.CloudItems = cloudItems;
        }

        public IEnumerable<T> CloudItems { get; }
    }
}