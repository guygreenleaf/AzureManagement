using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace AzureMgmt.AzureService
{
    public class AzureService : IAzureService
    {
        private readonly string CONNECTION_STRING;
        private readonly BlobServiceClient blobServiceClient;
        private readonly BlobContainerClient blobContainerClient;

#if DEBUG
        public AzureService(IConfiguration configuration)
        {

            CONNECTION_STRING = configuration.GetConnectionString("Azure");

            if (string.IsNullOrEmpty(CONNECTION_STRING)) throw new AccessViolationException("Connection string not found."); 

            blobServiceClient = new BlobServiceClient(CONNECTION_STRING);
            if (blobServiceClient is null) throw new MemberAccessException("Blob Services not found.");

            blobContainerClient = new BlobContainerClient(CONNECTION_STRING, "publicfilesggreenleaf");
        }
#else
        public AzureService()
        {
            CONNECTION_STRING = Environment.GetEnvironmentVariable("CONNECTION_STRING"); 
            if (string.IsNullOrEmpty(CONNECTION_STRING)) throw new AccessViolationException("Connection string not found."); 

            blobServiceClient = new BlobServiceClient(CONNECTION_STRING);
            if (blobServiceClient is null) throw new MemberAccessException("Blob Services not found.");

            blobContainerClient = new BlobContainerClient(CONNECTION_STRING, "publicfilesggreenleaf");
        }
#endif

        public async Task<List<BlobItem>> GetBlobs()
        {
            var ctr = new List<BlobItem>();
            var resultSegment = blobContainerClient.GetBlobsAsync().AsPages(default, 1);

            await foreach (Azure.Page<BlobItem> blobPage in resultSegment)
            {
                foreach (BlobItem blobItem in blobPage.Values)
                {
                    ctr.Add(blobItem);               
                }
            }       
            return ctr;
        } 
    }
}
