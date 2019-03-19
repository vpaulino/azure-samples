using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Abstractions;
using Azure.Storage.Abstractions.Blobs;

namespace Azure.Storage.Blobs
{
    public interface IBlobStorageProvider
    {
        Task<BlobDescriptionDetails> DeleteBlob(string location, string fileName, CancellationToken ct);
        Task<BlobDescriptionDetails> Download(string location, string fileName, Stream destination, CancellationToken ct);

        Task<BlobDescriptionDetails> Upload(Stream stream, string location, string fileName, string contentType, CancellationToken ct);

        Task<BlobDescriptionDetails> GetBlobDetails(string location, string name, CancellationToken ct);
        Task<ListResult<BlobDescription>> ListBlobs(string location, CancellationToken ct);
        Task<ListResult<ContainerItem>> ListContainers(CancellationToken ct);
        Task MoveBlob(string currentlocation, string newLocation, string fileName, CancellationToken ct);
        Task RenameBlob(string location, string currentFileName, string newFileName, CancellationToken ct);
      
    }
}