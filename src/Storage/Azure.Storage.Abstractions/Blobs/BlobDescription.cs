using System;
using System.Linq;

namespace Azure.Storage.Abstractions.Blobs
{
    public class BlobDescription
    {
        public BlobDescription(Uri uri, string containerName, bool isDirectory)
        {
            this.Uri = uri;
            this.ContainerName = containerName;
            this.FileName = this.Uri.AbsoluteUri.Split('/').Last() ;
            this.IsDirectory = isDirectory;
        }

        public Uri Uri { get;  }
        public string ContainerName { get; }
        public string FileName { get; }
        public bool IsDirectory { get; }
    }
}