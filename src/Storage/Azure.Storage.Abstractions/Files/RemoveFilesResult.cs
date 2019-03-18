using System;
using System.Collections.Generic;

namespace Azure.Storage.Abstractions.Files
{
    public class RemoveFilesResult
    {
        private List<Tuple<string, bool>> filesDeleted = new List<Tuple<string, bool>>();
        public RemoveFilesResult()
        {

        }
        public void Add(string file, bool deleted)
        {
            filesDeleted.Add(new Tuple<string, bool>(file, deleted));
        }
    }
}