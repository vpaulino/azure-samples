using Azure.Storage.Abstractions.Files;
using System;
using System.Collections.Generic;


namespace Azure.Storage.Files
{
    public class FileDescription : ContentDescription
    {
         

        public FileDescription(string shareName, string directoryName, string fileName, Uri uri, long length, string eTag, DateTimeOffset? lastModified, IDictionary<string, string> metadata)
            : base(shareName, uri, eTag, lastModified, metadata, false)
        {
           
            DirectoryName = directoryName;
            FileName = fileName;
            Length = length;
            
        }



      
        public string DirectoryName { get; }
        public string FileName { get; }
        
        public long Length { get; }
       
       
      
    }
}