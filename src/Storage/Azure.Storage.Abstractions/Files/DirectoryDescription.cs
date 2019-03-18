using System;
using System.Collections.Generic;

namespace Azure.Storage.Abstractions.Files
{
    public class DirectoryDescription : ContentDescription
    {
        
       
        public DirectoryDescription(string shareName, string name, Uri uri, string eTag, DateTimeOffset? lastModified, IDictionary<string, string> metadata) : base(shareName, uri, eTag, lastModified, metadata, true)
        {
          
            this.Name = name;
           
        }
         
        public string Name { get;   }
      
    }
}