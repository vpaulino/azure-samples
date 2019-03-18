using System;
using System.Collections.Generic;
using System.Text;

namespace Azure.Storage.Abstractions.Files
{
    public class ContentDescription
    {
       

        public ContentDescription(string shareName, Uri uri, string eTag, DateTimeOffset? lastModified, IDictionary<string, string> metadata, bool IsDirectory)
        {
            this.ShareName = shareName;
            this.Uri = uri;
            this.ETag = eTag;
            this.LastModified = lastModified;
            this.Metadata = metadata;
        }

        public string ShareName { get; }
        public Uri Uri { get; }
        public string ETag { get; }
        public DateTimeOffset? LastModified { get; }

        public IDictionary<string, string> Metadata { get; }
    }
}
