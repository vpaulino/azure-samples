using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Azure.Storage.Abstractions.Blobs
{
    public interface IBlobStorageProvider
    {

        Task<UploadResult> Upload(Stream stream, string location, string fileName, CancellationToken ct);

        Task<DownloadResult> Download(string location, string fileName, Stream destination, CancellationToken ct);


        
    }
}