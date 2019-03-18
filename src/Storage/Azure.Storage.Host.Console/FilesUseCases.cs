using Azure.Storage.Abstractions;
using Azure.Storage.Abstractions.Blobs;
using Azure.Storage.Abstractions.Files;
using Azure.Storage.Blobs;
using Azure.Storage.Files;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Azure.Storage.Host.UploadClient
{
    public static class FilesUseCases
    {

        private static Stream GetFileToUpload(Uri filename)
        {
            return File.Open(filename.AbsolutePath, FileMode.Open);
        }

        public static async Task Delete(FileDescription fileDetails)
        {
            var fileStorage = new FileStorageProvider(new StorageSettings(ConfigurationManager.AppSettings.Get("StorageConnectionString")));
            var result = await fileStorage.RemoveFile(fileDetails.ShareName, fileDetails.DirectoryName, fileDetails.FileName, CancellationToken.None);
        }

        public static async Task Download(FileDescription fileDetails, string destinationFolder)
        {
            var blobStorage = new FileStorageProvider(new StorageSettings(ConfigurationManager.AppSettings.Get("StorageConnectionString")));
            var fullFileName = $"{destinationFolder}/{fileDetails.FileName}";
            using (FileStream fs = new FileStream(fullFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                var result = await blobStorage.GetFile(fileDetails.ShareName, fileDetails.DirectoryName, fileDetails.FileName, fs, CancellationToken.None);
                await fs.FlushAsync();
            }

            if (!File.Exists(fullFileName))
            {
                throw new OperationCanceledException("File was not downloaded as expected");
            }
        }

        public static async Task<ListResult<ContentDescription>> GetFilesDescription(string root, string folder, CancellationToken ct)
        {
            var blobStorage = new FileStorageProvider(new StorageSettings(ConfigurationManager.AppSettings.Get("StorageConnectionString")));
            return await blobStorage.ListAllContent(root, folder, ct);
        }

        public static async Task<BlobDescriptionDetails> GetBlobDetails(string folder, string filename)
        {
            var blobStorage = new BlobStorageProvider(new StorageSettings(ConfigurationManager.AppSettings.Get("StorageConnectionString")));
            var result = await blobStorage.GetBlobDetails(folder, filename, CancellationToken.None);
            return result;
        }

        public static async Task UploadBlob(string folder, string filename, string destinationLocation)
        {
            using (Stream stream = GetFileToUpload(new Uri($"{folder}/{filename}")))
            {
                var blobStorage = new BlobStorageProvider(new StorageSettings(ConfigurationManager.AppSettings.Get("StorageConnectionString")));
                var result = await blobStorage.Upload(stream, destinationLocation, filename, CancellationToken.None);
                ReportUploadResult(result);
            }
        }
         

        private static void ReportUploadResult(UploadResult result)
        {
            Console.WriteLine(result);
        }

        //public static async Task UploadFile(string folder, string filename, string destinationLocation)
        //{
        //    using (Stream stream = GetFileToUpload(new Uri($"{folder}/{filename}")))
        //    {
        //        var fileStorage = new FileStorageProvider(new StorageSettings(ConfigurationManager.AppSettings.Get("StorageConnectionString")));
        //        var fileDescription = await fileStorage.CreateFile(folder, destinationLocation, filename, stream, CancellationToken.None);
        //        ReportUploadResult(fileDescription);
        //    }
        //}
    }
}
