using Azure.Storage.Abstractions;
using Azure.Storage.Abstractions.Blobs;
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

            await Blobs(folder, filename, destinationLocation);

          //  await Files(folder, filename, destinationLocation);
        }

        //private static async Task Files(string folder, string filename, string destinationLocation)
        //{
        //    await UploadFile(folder, filename, destinationLocation);
        //    var blobs = await GetBlobs(destinationLocation);

        //    foreach (var blob in blobs.CloudItems)
        //    {
        //        FileDescription filesDetails = await GetFileDetails(blob.ContainerName, blob.FileName);
        //        Console.WriteLine(blobDetails);
        //        await Download(blobDetails, Environment.CurrentDirectory);
        //        await Delete(blobDetails);
        //    }
        //}

        private static async Task Blobs(string folder, string filename, string destinationLocation)
        {
            await BlobsUseCases.UploadBlob(folder, filename, destinationLocation);
            var blobs = await BlobsUseCases.GetBlobs(destinationLocation);

            foreach (var blob in blobs.CloudItems)
            {
                BlobDescriptionDetails blobDetails = await BlobsUseCases.GetBlobDetails(blob.ContainerName, blob.FileName);
                Console.WriteLine(blobDetails);
                await BlobsUseCases.Download(blobDetails, Environment.CurrentDirectory);
                await BlobsUseCases.Delete(blobDetails);
            }
        }

      

        private static void ReportUploadResult(FileDescription fileDescription)
        {
            Console.WriteLine(fileDescription);
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
