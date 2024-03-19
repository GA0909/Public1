using AteaScraper.IServices;
using Azure;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AteaScraper.Services
{
    public class AzureStorageService : IAzureStorageService
    {
        private readonly TableServiceClient _tableServiceClient;
        private readonly BlobContainerClient _blobContainerClient;

        public AzureStorageService(TableServiceClient tableServiceClient, BlobContainerClient blobContainerClient)
        {
            _tableServiceClient = tableServiceClient;
            _blobContainerClient = blobContainerClient;
        }
        public async Task<bool> AddEntityToTableAsync(string partitionKey, string rowKey, bool isSuccess, Guid requestKey)
        {
            try
            {
                var tableClient = _tableServiceClient.GetTableClient("atea");
                await tableClient.CreateIfNotExistsAsync();

                var tableEntity = new TableEntity(partitionKey, rowKey)
            {
                { "Request", requestKey },
                { "status", isSuccess }
            };

                await tableClient.AddEntityAsync(tableEntity);
                return true; // Return true if entity added successfully
            }
            catch (RequestFailedException ex)
            {
                // Log or handle the exception
                Console.WriteLine($"Error adding entity to Azure Table: {ex.Message}");
                return false; // Return false if an error occurred
            }
        }

        public async Task<bool> UploadBlobAsync(string blobName, Stream stream)
        {
            try
            {
                var blobClient = _blobContainerClient.GetBlobClient(blobName);
                await blobClient.UploadAsync(stream, true);
                return true; // Return true if blob uploaded successfully
            }
            catch (RequestFailedException ex)
            {
                // Log or handle the exception
                Console.WriteLine($"Error uploading blob to Azure Storage: {ex.Message}");
                return false; // Return false if an error occurred
            }
        }
    }
}
