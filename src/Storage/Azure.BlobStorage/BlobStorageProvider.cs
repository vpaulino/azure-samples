using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Azure.Storage.Blobs
{
    public class BlobStorageProvider : StorageProvider, IBlobStorageProvider
    {
       
        public BlobStorageProvider(StorageSettings settings) : base(settings)
        {
            
        }

        private async Task<CloudBlobContainer> GetOrCreateContainer(string location, CloudBlobClient blobClient)
        {
            CloudBlobContainer container = blobClient.GetContainerReference(location);

            await container.CreateIfNotExistsAsync();
            return container;
        }

        protected virtual CloudBlobContainer GetBlobContainer(string location, CloudStorageAccount storageAccount)
        {
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            // Get reference to the blob container by passing the name by reading the value from the configuration (appsettings.json)
            return blobClient.GetContainerReference(location);
        }


      



        public async Task<ListResult<ContainerItem>> ListContainers(CancellationToken ct)
        {
            CloudStorageAccount storageAccount = GetCloudStorageAccount();

            var client = storageAccount.CreateCloudBlobClient();

            BlobContinuationToken blobContinuationToken = null;
            List<CloudBlobContainer> cloudBlobContainers = new List<CloudBlobContainer>();

            do
            {
                var result = await client.ListContainersSegmentedAsync(blobContinuationToken, ct);

                blobContinuationToken = result.ContinuationToken;

                cloudBlobContainers.AddRange(result.Results);

            } while (blobContinuationToken != null);
            
            return new ListResult<ContainerItem>(cloudBlobContainers.Select((container) => new ContainerItem(container.Uri, container.Name)));
        }

        public async Task RenameBlob(string location, string oldFileName, string newFileName, CancellationToken ct)
        {
            CloudStorageAccount storageAccount = GetCloudStorageAccount();

            CloudBlobContainer container = GetBlobContainer(location, storageAccount);

            CloudBlockBlob blobCopy = container.GetBlockBlobReference(newFileName);

            if (!await blobCopy.ExistsAsync())
            {
                CloudBlockBlob blob = container.GetBlockBlobReference(oldFileName);

                if (await blob.ExistsAsync(ct))
                {
                    await blobCopy.StartCopyAsync(blob, ct);
                    await blob.DeleteIfExistsAsync();
                }
            }
        }

        public async Task MoveBlob(string currentlocation, string newLocation, string fileName, CancellationToken ct)
        {
            CloudStorageAccount storageAccount = GetCloudStorageAccount();

            CloudBlobContainer container = GetBlobContainer(currentlocation, storageAccount);

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer newContainer = await GetOrCreateContainer(newLocation, blobClient);

            CloudBlockBlob blobCopy = newContainer.GetBlockBlobReference(fileName);

            if (!await blobCopy.ExistsAsync())
            {
                CloudBlockBlob blob = container.GetBlockBlobReference(fileName);

                if (await blob.ExistsAsync(ct))
                {
                    await blobCopy.StartCopyAsync(blob, ct);
                    await blob.DeleteIfExistsAsync();
                }
            }
        }


        public async Task<ListResult<BlobItem>> ListBlobs(string location,  CancellationToken ct)
        {
            CloudStorageAccount storageAccount = GetCloudStorageAccount();

            CloudBlobContainer container = GetBlobContainer(location, storageAccount);

            BlobContinuationToken blobContinuationToken = null;

            List<IListBlobItem> listResult = new List<IListBlobItem>();
            
            do
            {
                
                var result = await container.ListBlobsSegmentedAsync(blobContinuationToken, ct);
                
                blobContinuationToken = result.ContinuationToken;

                listResult.AddRange(result.Results);

            } while (blobContinuationToken != null);
             

            return new ListResult<BlobItem>(listResult.Select((listBlobItem)=> new BlobItem(listBlobItem.Uri, listBlobItem.Container.Name)));
        }

        public async Task<BlobItemDetails> GetBlobDetails(string location, string name, CancellationToken ct)
        {
            CloudStorageAccount storageAccount = GetCloudStorageAccount();

            CloudBlobContainer container = GetBlobContainer(location, storageAccount);

            var blob = await container.GetBlobReferenceFromServerAsync(name);

            return new BlobItemDetails(blob.Uri, blob.Container.Name, blob.Properties.BlobType, blob.Properties.ContentEncoding, blob.Properties.ContentLanguage, blob.Properties.Created, blob.Properties.ETag, blob.Properties.LastModified, blob.Properties.Length, blob.Properties.LeaseStatus, blob.Properties.LeaseDuration);

        }

        
        public async Task<DeleteResult> DeleteBlob(string location, string fileName, CancellationToken ct)
        {
            CloudStorageAccount storageAccount = GetCloudStorageAccount();
             
            CloudBlobContainer container = GetBlobContainer(location, storageAccount);

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);

            var result = await blockBlob.DeleteIfExistsAsync(ct);

            return new DeleteResult(result, blockBlob.Uri);

        }

        public async Task<DownloadResult> Download(string location, string fileName, Stream destination, CancellationToken ct)
        {

            CloudStorageAccount storageAccount = GetCloudStorageAccount();
            // Create the blob client.
            CloudBlobContainer container = GetBlobContainer(location, storageAccount);

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);

            await blockBlob.DownloadToStreamAsync(destination, ct);

            return new DownloadResult(blockBlob.Uri);


        }
        
        public async Task<UploadResult> Upload(Stream stream, string location, string fileName, CancellationToken ct)
        {

            CloudStorageAccount storageAccount = GetCloudStorageAccount();
            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            // Get reference to the blob container by passing the name by reading the value from the configuration (appsettings.json)
            CloudBlobContainer container = await GetOrCreateContainer(location, blobClient);
            // Get the reference to the block blob from the container
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);
            // Upload the file
            await blockBlob.UploadFromStreamAsync(stream, ct);

            return new UploadResult(blockBlob.Uri, location, fileName);


        }

    }
}
