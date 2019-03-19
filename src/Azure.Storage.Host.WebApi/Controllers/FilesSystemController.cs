using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Files;
using Microsoft.AspNetCore.Mvc;

namespace Azure.Storage.Host.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesSystemController : ControllerBase
    {
        private readonly IFileStorageProvider filesStorageProvider;

        // GET api/valuess

        public FilesSystemController(IFileStorageProvider filesStorageProvider)
        {
            this.filesStorageProvider = filesStorageProvider;
        }

        /// <summary>
        /// Gets all the content description from the folder that must exist in the share folder.
        /// </summary>
        /// <param name="share"></param>
        /// <param name="location"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("{share}/contents")]
        public async Task<IActionResult> GetContentDetails(string share, [FromQuery]string location, CancellationToken ct)
        {

            var allContent = await this.filesStorageProvider.ListAllContent(share, location, ct);

            return Ok(allContent);

        }
       
    }
}
