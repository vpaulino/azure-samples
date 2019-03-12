using System;
using System.Linq;

namespace Azure.Storage.Blobs
{
    public class BlobItem
    {
        public BlobItem(Uri uri, string containerName)
        {
            this.Uri = uri;
            this.ContainerName = containerName;
            this.FileName = this.Uri.AbsoluteUri.Split('/').Last() ;
        }

        public Uri Uri { get;  }
        public string ContainerName { get; }
        public string FileName { get; }

    }
}