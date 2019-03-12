using System;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Azure.Storage.Blobs
{
    public class BlobItemDetails : BlobItem
    {
        

         

        public BlobItemDetails(Uri uri, string containerName, BlobType blobType, string contentEncoding, string contentLanguage, DateTimeOffset? created, string eTag, DateTimeOffset? lastModified, long length, LeaseStatus leaseStatus, LeaseDuration leaseDuration) : base(uri, containerName)
        {
            this.ContentEncoding = contentEncoding;
            this.ContentLanguage = contentLanguage;
            this.Created = created;
            this.ETag = eTag;
            this.LastModified = lastModified;
            this.Length = length;
            this.LeaseStatus = leaseStatus;
            this.LeaseDuration = leaseDuration;
        }

        public string ContentEncoding { get;  }
        public string ContentLanguage { get; }
        public DateTimeOffset? Created { get; }
        public string ETag { get; }
        public DateTimeOffset? LastModified { get; }
        public long Length { get; }
        public LeaseStatus LeaseStatus { get; }
        public LeaseDuration LeaseDuration { get; }

        public override string ToString()
        {
            return $"{{ Uri:\"{this.Uri}\", Created:\"{Created}\", Length:\"{Length}\" }}";
        }
    }
}