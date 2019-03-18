using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Abstractions;
using Azure.Storage.Abstractions.Files;

namespace Azure.Storage.Files
{
    public interface IFileStorageProvider
    {
        Task<FileDescription> CopyFile(string root, string currentDirectory, string newDirectory, string file, CancellationToken ct);
        Task<DirectoryDescription> CreateDirectory(string root, string directory, CancellationToken ct);
        Task<FileDescription> CreateFile(string root, string directory, string name, Stream file, CancellationToken ct);
        Task<IEnumerable<FileDescription>> CreateFiles(string root, string directory, IEnumerable<Tuple<string, Stream>> files, CancellationToken ct);
         Task<ListResult<FileDescription>> GetFiles(string root, string directory, IEnumerable<Tuple<string, Stream>> files, CancellationToken ct);
        Task<FileDescription> MoveFile(string root, string currentDirectory, string newDirectory, string file, CancellationToken ct);
        Task<DirectoryDescription> RemoveDirectory(string root, string directory, CancellationToken ct);
        Task<bool> RemoveFile(string root, string directory, string file, CancellationToken ct);
        Task<RemoveFilesResult> RemoveFiles(string root, string directory, IEnumerable<string> files, CancellationToken ct);
    }
}