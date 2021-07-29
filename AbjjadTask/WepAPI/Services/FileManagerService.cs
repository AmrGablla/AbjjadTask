using Azure.Storage.Blobs;
using System;
using System.IO;
using System.Threading.Tasks;
using WepAPI.DTOs;
using WepAPI.Interfaces;

namespace WepAPI.Services
{
    public class FileManagerService : IFileManagerService
    {
        private readonly BlobServiceClient _blobServiceClient;
        public FileManagerService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task Upload(ImageDTO model)
        {
            try
            {
                if (model is null)
                {
                    throw new ArgumentNullException(nameof(model));
                }

                if (string.IsNullOrEmpty(model.FileContent))
                {
                    throw new ArgumentNullException(nameof(model.FileContent));
                }

                GenrateRandomName(model);

                byte[] bytes = Convert.FromBase64String(model.FileContent);
                var blobContainer = _blobServiceClient.GetBlobContainerClient("images");

                var blobClient = blobContainer.GetBlobClient(model.FileContent);
                using (var stream = new MemoryStream(bytes))
                {
                    await blobClient.UploadAsync(stream);
                }
            }
            catch (Exception ex)
            {
                throw;
            } 
        }

        private static void GenrateRandomName(ImageDTO model)
        {
            Guid g = Guid.NewGuid();
            string GuidString = Convert.ToBase64String(g.ToByteArray());
            GuidString = GuidString.Replace("=", "");
            GuidString = GuidString.Replace("+", "");
            model.FileName = GuidString;
        }

        public async Task<byte[]> Get(string imageName)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient("images");

            var blobClient = blobContainer.GetBlobClient(imageName);
            var downloadContent = await blobClient.DownloadAsync();
            using (MemoryStream ms = new MemoryStream())
            {
                await downloadContent.Value.Content.CopyToAsync(ms);
                return ms.ToArray();
            }
        }
    }
}
