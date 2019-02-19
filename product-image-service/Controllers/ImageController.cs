using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using product_image_service.Data;

namespace product_image_service.Controllers
{
    public class ImageController : Controller
    {
        private IStorageClient _storageClient;

        public ImageController(IStorageClient storageClient)
        {
            _storageClient = storageClient;
        }

        [HttpPost]
        [Route("api/[controller]/{*url}")]
        public async Task<IActionResult> Upload([FromRoute]string url, IFormFile file)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                url = "";
            }
            if (file == null || file.Length <= 0)
            {
                return BadRequest("Must include a file under the POST form key 'file'.");
            }
            var filepath = Path.Join(url, file.FileName);
            await _storageClient.UploadFromStreamAsync(filepath, file.OpenReadStream());
            return Ok($"Uploaded file '{file.FileName}' to path '{Path.Join(url, file.FileName)}'");
        }

        [HttpGet]
        [Route("api/[controller]/{*url}")]
        public async Task<IActionResult> Download([FromRoute]string url)
        {
            return File(await _storageClient.OpenReadAsync(url), "application/octet-stream", Path.GetFileName(url));
        }
    }
}
