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
    class Program
    {
        static void Main(string[] args)
        {

           
            MainAsync(args).Wait();

        }

        private static async Task MainAsync(string[] args)
        {
            string folder = GetFolder(args);
            string filename = GetFileNameToUpload(args);
            string destinationLocation = GetDestinationLocation(args);

            await Upload(folder, filename, destinationLocation);
            var blobs = await GetBlobs(destinationLocation);

            foreach (var blob in blobs.CloudItems)
            {
                BlobItemDetails blobDetails = await GetBlobDetails(blob.ContainerName, blob.FileName);
                Console.WriteLine(blobDetails);
                await Download(blobDetails, Environment.CurrentDirectory);
                await Delete(blobDetails);
            }
        }

        private static async Task Delete(BlobItemDetails blobDetails)
        {
            var blobStorage = new BlobStorageProvider(new StorageSettings(ConfigurationManager.AppSettings.Get("StorageConnectionString")));
            var result = await blobStorage.DeleteBlob(blobDetails.ContainerName, blobDetails.FileName, CancellationToken.None);
            
        }

        private static async Task Download(BlobItemDetails blobDetails, string destinationFolder)
        {
            var blobStorage = new BlobStorageProvider(new StorageSettings(ConfigurationManager.AppSettings.Get("StorageConnectionString")));
            var fullFileName = $"{destinationFolder}/{blobDetails.FileName}";
            using (FileStream fs = new FileStream(fullFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
              var result =  await blobStorage.Download(blobDetails.ContainerName, blobDetails.FileName, fs, CancellationToken.None);
              await fs.FlushAsync();
            }

            if (!File.Exists(fullFileName))
                throw new OperationCanceledException("File was not downloaded as expected");
        }

        private static async Task<ListResult<BlobItem>> GetBlobs(string folder)
        {
            var blobStorage = new BlobStorageProvider(new StorageSettings(ConfigurationManager.AppSettings.Get("StorageConnectionString")));
            return await blobStorage.ListBlobs(folder, CancellationToken.None);
            
        }

        private static async Task<BlobItemDetails> GetBlobDetails(string folder, string filename)
        {
            var blobStorage = new BlobStorageProvider(new StorageSettings(ConfigurationManager.AppSettings.Get("StorageConnectionString")));
            var result = await blobStorage.GetBlobDetails(folder, filename, CancellationToken.None);
            return result;
        }

        private static async Task Upload(string folder, string filename, string destinationLocation)
        {
            using (Stream stream = GetFileToUpload(new Uri($"{folder}/{filename}")))
            {
                var blobStorage = new BlobStorageProvider(new StorageSettings(ConfigurationManager.AppSettings.Get("StorageConnectionString")));
                var result = await blobStorage.Upload(stream, destinationLocation, filename, CancellationToken.None);
                ReportUploadResult(result);
            }
        }

        private static string GetDestinationLocation(string[] args)
        {
            return args[2];
        }

        private static string GetFolder(string[] args)
        {
            return args[0];
        }

        private static void ReportUploadResult(UploadResult result)
        {
            Console.WriteLine(result);
        }

        private static Stream GetFileToUpload(Uri filename)                                               
        {
            return File.Open(filename.AbsolutePath, FileMode.Open);
        }

        private static string GetFileNameToUpload(string[] args)
        {
            return args[1];
        }
    }
}
