using Azure.Storage.Abstractions;
using Azure.Storage.Abstractions.Blobs;
using Azure.Storage.Blobs;
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
    public static class BlobsUseCases
    {

        private static Stream GetFileToUpload(Uri filename)
        {
            return File.Open(filename.AbsolutePath, FileMode.Open);
        }

        public static async Task Delete(BlobDescriptionDetails blobDetails)
        {
            var blobStorage = new BlobStorageProvider(new StorageSettings(ConfigurationManager.AppSettings.Get("StorageConnectionString")));
            var result = await blobStorage.DeleteBlob(blobDetails.ContainerName, blobDetails.FileName, CancellationToken.None);
        }

        public static async Task Download(BlobDescriptionDetails blobDetails, string destinationFolder)
        {
            var blobStorage = new BlobStorageProvider(new StorageSettings(ConfigurationManager.AppSettings.Get("StorageConnectionString")));
            var fullFileName = $"{destinationFolder}/{blobDetails.FileName}";
            using (FileStream fs = new FileStream(fullFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                var result = await blobStorage.Download(blobDetails.ContainerName, blobDetails.FileName, fs, CancellationToken.None);
                await fs.FlushAsync();
            }

            if (!File.Exists(fullFileName))
            {
                throw new OperationCanceledException("File was not downloaded as expected");
            }
        }

        public static async Task<ListResult<BlobDescription>> GetBlobs(string folder)
        {
            var blobStorage = new BlobStorageProvider(new StorageSettings(ConfigurationManager.AppSettings.Get("StorageConnectionString")));
            return await blobStorage.ListBlobs(folder, CancellationToken.None);

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
