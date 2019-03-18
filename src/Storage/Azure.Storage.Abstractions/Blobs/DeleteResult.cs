using System;

namespace Azure.Storage.Abstractions.Blobs
{
    public class DeleteResult
    {

        public DeleteResult(bool deleted, Uri fileDeleted)
        {
            this.Deleted = deleted;
            this.FileDeleted = fileDeleted;
        }
        public bool Deleted { get; }

        public Uri FileDeleted { get;  }
    }
}