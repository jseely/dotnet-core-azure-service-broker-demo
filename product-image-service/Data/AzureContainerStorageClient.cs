using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;

namespace product_image_service.Data
{
    public class AzureContainerStorageClient : IStorageClient
    {
        private CloudBlobContainer _container;

        public AzureContainerStorageClient(CloudBlobContainer container)
        {
            _container = container;
        }

        public Task UploadFromStreamAsync(string filename, Stream stream)
        {
            return UploadFromStreamAsync(filename, stream, default(CancellationToken));
        }

        public Task UploadFromStreamAsync(string filename, Stream stream, CancellationToken token = default(CancellationToken))
        {
            var blob = _container.GetBlockBlobReference(filename);
            return blob.UploadFromStreamAsync(stream);
        }

        public Task<Stream> OpenReadAsync(string filename)
        {
            return OpenReadAsync(filename, default(CancellationToken));
        }

        public Task<Stream> OpenReadAsync(string filename, CancellationToken token = default(CancellationToken))
        {
            var blob = _container.GetBlobReference(filename);
            return blob.OpenReadAsync();
        }
    }
}