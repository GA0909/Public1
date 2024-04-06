using AteaScraper.IServices;
using Azure;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace AteaScraper.Services
{
    
    public class AzureStorageService : IAzureStorageService
    {
        private readonly IConfiguration _config;
        public AzureStorageService(IConfiguration config)
        {
            _config = config;
        }
        public async Task<bool> AddEntityToTableAsync(string partitionKey, string rowKey, bool isSuccess, Guid requestKey)
        {
            try
            {
                var serviceClient = new TableServiceClient(_config["AzureWebJobsStorage"]);
                var table = serviceClient.GetTableClient(_config["MyTableName"]);
                await table.CreateIfNotExistsAsync();

                var tableEntity = new TableEntity(partitionKey, rowKey)
                {
                    { "Request", requestKey },
                    { "status", isSuccess }
                };

                await table.AddEntityAsync(tableEntity);
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
                var blobClient = new BlobContainerClient(_config["AzureWebJobsStorage"], _config["MyTableName"]);
                await blobClient.CreateIfNotExistsAsync();
                var blob = blobClient.GetBlobClient(blobName);
                await blob.UploadAsync(stream);

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
