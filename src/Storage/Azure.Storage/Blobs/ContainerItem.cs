using System;
using System.Linq;

namespace Azure.Storage.Blobs
{
    public class ContainerItem
    {
        public ContainerItem(Uri uri, string containerName)
        {
            this.Uri = uri;
            this.ContainerName = containerName;
        }

        public Uri Uri { get;  }
        public string ContainerName { get; }
       

    }
}