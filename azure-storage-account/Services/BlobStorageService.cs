using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Microsoft.AspNetCore.Http;

namespace azure_storage_account.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly IConfiguration _configuration;
        private string containerName = "attendeeimages";
        public BlobStorageService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> UploadBlob(IFormFile formFile, string imageName, string? originalBlobName = null)
        {
            var blobName = $"{imageName}{Path.GetExtension(formFile.FileName)}";
            var container = await GetBlobContainerClient();

            if (!string.IsNullOrEmpty(originalBlobName))
            {
                await RemoveBlob(originalBlobName);
            }

            using (var memoryStream = new MemoryStream())
            {
                formFile.CopyTo(memoryStream);
                memoryStream.Position = 0;

                var blob = container.GetBlobClient(blobName);
                await blob.UploadAsync(content: memoryStream, overwrite: true);
                return blobName;
            }
        }
        public async Task<string> GetBlobUrl(string imageName)
        {
            var container = await GetBlobContainerClient();

            var blob = container.GetBlobClient(imageName);

            BlobSasBuilder blobSasBuilder = new()
            {
                BlobContainerName = blob.BlobContainerName,
                BlobName = blob.Name,
                ExpiresOn = DateTime.UtcNow.AddMinutes(2),
                Protocol = SasProtocol.Https,
                Resource = "b"//resourse type
            };
            blobSasBuilder.SetPermissions(BlobAccountSasPermissions.Read);

            return blob.GenerateSasUri(blobSasBuilder).ToString();
        }
        public async Task RemoveBlob(string imageName)
        {
            var container = await GetBlobContainerClient();
            var blob = container.GetBlobClient(imageName);
            await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
        }
        private async Task<BlobContainerClient> GetBlobContainerClient()
        {
            try
            {
                BlobContainerClient container = new BlobContainerClient(_configuration["StorageConnectionString"], containerName);
                await container.CreateIfNotExistsAsync();

                return container;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
