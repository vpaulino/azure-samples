using System;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Azure.Storage.Abstractions.Blobs
{
    public class BlobDescriptionDetails : BlobDescription
    {
        public BlobDescriptionDetails(Uri uri, string containerName, bool isDirectory, string contentType, string contentLanguage, DateTimeOffset? created, string eTag, DateTimeOffset? lastModified, long length, string leaseStatus, string leaseState, string leaseDuration) : base(uri, containerName, isDirectory)
        {
            this.ContentType = contentType;
            this.ContentLanguage = contentLanguage;
            this.Created = created;
            this.ETag = eTag;
            this.LastModified = lastModified;
            this.Length = length;
            this.LeaseStatus = leaseStatus;
            this.LeaseState = leaseState;
            this.LeaseDuration = leaseDuration;
        }

        public string ContentType { get;  }
        public string ContentLanguage { get; }
        public DateTimeOffset? Created { get; }
        public string ETag { get; }
        public DateTimeOffset? LastModified { get; }
        public long Length { get; }
        public string LeaseStatus { get; }

        public string LeaseState { get; set; }

        public string LeaseDuration { get; }

        public override string ToString()
        {
            return $"{{ Uri:\"{this.Uri}\", Created:\"{Created}\", Length:\"{Length}\" }}";
        }
    }
}