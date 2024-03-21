using AteaScraper.Mediators;
using AteaScraper.Validators;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AteaScraper.Blobing
{
    public class BlobQueryHandler : IRequestHandler<BlobQueryRequest, IActionResult>
    {
        private readonly BlobContainerClient _blobContainerClient;

        public BlobQueryHandler(BlobContainerClient blobContainerClient)
        {
            _blobContainerClient = blobContainerClient;
        }

        public async Task<IActionResult> Handle(BlobQueryRequest request)
        {
            // Validate query parameters
            var validator = new BlobQueryValidator();
            if (!validator.Validate(request))
            {
                return new BadRequestResult();
            }
            // Get the BlobClient for the specified blob
            var blobClient = _blobContainerClient.GetBlobClient($"{request.BlobName}.json");

            // Check if blob exists
            if (!await blobClient.ExistsAsync())
            {
                return new NotFoundResult();
            }

            // Download the result of search
            var blobResponse = await blobClient.DownloadContentAsync();

            // Return the results of search
            return new OkObjectResult(blobResponse.Value.Content.ToString());
        }

       
    }
}
