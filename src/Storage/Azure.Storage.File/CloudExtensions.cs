
using Azure.Storage.Abstractions.Files;
using Microsoft.WindowsAzure.Storage.File;
using System;
using System.Collections.Generic;
using System.Text;

namespace Azure.Storage.Files
{
    public static class CloudExtensions
    {

        public static FileDescription ConvertToFileDescription(this CloudFile fileReference, string directory)
        {
           return new FileDescription(directory, fileReference.Parent.Name, fileReference.Name, fileReference.Uri, fileReference.Properties.Length, fileReference.Properties.ETag, fileReference.Properties.LastModified, fileReference.Metadata);
        }

        public static DirectoryDescription ConvertToDirectoryDescription(this CloudFileDirectory dirReference, string root)
        {
            return new DirectoryDescription(root, dirReference.Name, dirReference.Uri, dirReference.Properties.ETag, dirReference.Properties.LastModified, dirReference.Metadata);
        }

        public static ContentDescription ConvertToContentDescription(this IListFileItem listFileItem, string root)
        {
            if (listFileItem is CloudFileDirectory)
            {
                var cloudFileDirectory = listFileItem as CloudFileDirectory;
                return new DirectoryDescription(root, cloudFileDirectory.Name, listFileItem.Uri, cloudFileDirectory.Properties.ETag, cloudFileDirectory.Properties.LastModified, cloudFileDirectory.Metadata);
            }

            if (listFileItem is CloudFile)
            {
                var cloudFile = listFileItem as CloudFile;
                return new DirectoryDescription(root, cloudFile.Name, listFileItem.Uri, cloudFile.Properties.ETag, cloudFile.Properties.LastModified, cloudFile.Metadata);
            }

            return null;

        }

    }
}
