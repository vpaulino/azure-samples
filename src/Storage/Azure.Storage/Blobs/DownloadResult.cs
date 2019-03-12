using System;

namespace Azure.Storage.Blobs
{
    public class DownloadResult
    {
        
        public DownloadResult()
        {

        }
        public DownloadResult(Exception ex)
        {
            this.Exception = ex;
        }

        public DownloadResult(Uri uri)
        {
            this.FileDonwloaded = uri;
        }

        public Uri FileDonwloaded { get;  }
        public Exception Exception { get;  }
    }
}