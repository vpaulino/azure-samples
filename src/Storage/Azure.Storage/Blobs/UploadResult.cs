using System;

namespace Azure.Storage.Blobs
{
    public class UploadResult
    {
         public UploadResult(Exception ex)
        {
            this.Exception = ex;
        } 
        
        public UploadResult(Uri fileLocation, string container, string fileName) 
        {
            this.FileLocation = fileLocation; 
            this.Container = container;
            this.FileName = fileName;
        }

        public string Container { get; }
        public string FileName { get; }

        public Uri FileLocation { get; }
        public Exception Exception { get; }

        public override string ToString()
        {
            return FileLocation.AbsolutePath;
        }
    }
}