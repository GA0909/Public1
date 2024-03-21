using System;
using System.IO;
using System.Threading.Tasks;

namespace AteaScraper.IServices
{
    public interface IAzureStorageService
    {
        Task<bool> AddEntityToTableAsync(string partitionKey, string rowKey, bool isSuccess, Guid requestKey);
        Task<bool> UploadBlobAsync(string blobName, Stream stream);
    }
}
