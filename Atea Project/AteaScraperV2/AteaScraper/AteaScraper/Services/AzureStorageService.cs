using AteaScraper.IServices;
using Azure;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AteaScraper.Services
{
    public class AzureStorageService : IAzureStorageService
    {
        public async Task<bool> AddEntityToTableAsync(string partitionKey, string rowKey, bool isSuccess, Guid requestKey)
        {
            try
            {

                var serviceClient = new TableServiceClient("UseDevelopmentStorage=true");
                var table = serviceClient.GetTableClient("atea");
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
                string connection = "UseDevelopmentStorage=true";
                string containerName = "atea";
                var blobClient = new BlobContainerClient(connection, containerName);
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
