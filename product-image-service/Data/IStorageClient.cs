using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace product_image_service.Data
{
    public interface IStorageClient
    {
        Task UploadFromStreamAsync(string filename, Stream stream);
        Task UploadFromStreamAsync(string filename, Stream stream, CancellationToken token);
        Task<Stream> OpenReadAsync(string filename);
        Task<Stream> OpenReadAsync(string filename, CancellationToken token);
    }
}