using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Abstractions.Blobs;
using Azure.Storage.Blobs;
using Azure.Storage.Files;
using Microsoft.AspNetCore.Mvc;

namespace Azure.Storage.Host.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlobsController : ControllerBase
    {
        private readonly IBlobStorageProvider storageProvider;

        // GET api/valuess

        public BlobsController(IBlobStorageProvider storageProvider)
        {
            this.storageProvider = storageProvider;
        }

        /// <summary>
        /// Gets all the content description from the folder that must exist in the share folder.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("{container}/contents")]
        public async Task<IActionResult> GetContentDetails(string container, CancellationToken ct)
        {
            var allContent = await this.storageProvider.ListBlobs(container, ct);
            return Ok(allContent);
        }

        [HttpPost("{container}/upload")]
        [Consumes("application/pdf", "image/jpeg", "application/zip", "application/msword", "application/octet-stream", "video/x-msvideo", "video/mpeg")]
        public async Task<IActionResult> UploadFile(Microsoft.AspNetCore.Http.IFormFile fileToUpload, string container, CancellationToken ct)
        {
            using (var stream = fileToUpload.OpenReadStream())
            {
                var uploadRequestHeaders = fileToUpload.Headers;
                var allContent = await this.storageProvider.Upload(stream, container, fileToUpload.FileName, fileToUpload.ContentType,  ct);
                return Ok(allContent);
            }
            
        }

        [HttpGet("{container}/download")]
         [Produces("application/pdf", "image/jpeg", "application/zip", "application/msword", "application/octet-stream", "video/x-msvideo", "video/mpeg")]
        public async Task<IActionResult> DownloadFile(string container, string filename, CancellationToken ct)
        {
            var memoryStream = new MemoryStream();
            var downloadDetailDescription =  await this.storageProvider.Download(container, filename, memoryStream, ct);
            memoryStream.Seek(0, SeekOrigin.Begin);
            // this.Response.Headers.Add("Content-Disposition", new Microsoft.Extensions.Primitives.StringValues(new string[] { $"attachment; filename={downloadDetailDescription.FileName}" }));
            //return new FileStreamResult(memoryStream, downloadDetailDescription.ContentType);
            return File(memoryStream, downloadDetailDescription.ContentType, downloadDetailDescription.FileName, downloadDetailDescription.LastModified, new Microsoft.Net.Http.Headers.EntityTagHeaderValue(downloadDetailDescription.ETag), false);
        }

    }
}
