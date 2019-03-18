using System.Collections.Generic;

namespace Azure.Storage.Abstractions
{
    public class ListResult<T>
    {
        private readonly List<T> cloudItems;

        public ListResult() : this(new List<T>()) { }
        public ListResult(IEnumerable<T> cloudItems)
        {
            this.cloudItems = new List<T>(cloudItems);
        }


        public void Add(T item)
        {
            this.cloudItems.Add(item);
        }


        public IEnumerable<T> CloudItems { get { return cloudItems; } }
    }
}