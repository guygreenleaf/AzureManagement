using Azure.Storage.Blobs;
using AzureMgmt.Util;

namespace AzureMgmt.AzureService
{
    public class AzureService : IAzureService
    {
        private readonly string? connectionString;
        private readonly BlobServiceClient blobServiceClient;
        private readonly BlobContainerClient blobContainerClient;
        private readonly string? baseBlobUri;

#if DEBUG
        private readonly IJSONUtils jsonUtils;
        public AzureService(IJSONUtils utils)
        {
            jsonUtils = utils;

            connectionString = jsonUtils.GetJSONItems().AzureConnectionString;

            if (string.IsNullOrEmpty(connectionString)) throw new AccessViolationException("Connection string not found.");

            blobServiceClient = new BlobServiceClient(connectionString);
            if (blobServiceClient is null) throw new MemberAccessException("Blob Services not found.");

            blobContainerClient = new BlobContainerClient(connectionString, jsonUtils.GetJSONItems().ContainerName);
            if (blobContainerClient is null) throw new MemberAccessException("Blob Container Services not found.");
            
            baseBlobUri = jsonUtils.GetJSONItems().BlobBaseURI;
            if (string.IsNullOrEmpty(baseBlobUri)) throw new MemberAccessException("Blob Base URI Not Found.");
        }
#else
        public AzureService()
        {
            connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING") ?? String.Empty; 
            if (string.IsNullOrEmpty(connectionString)) throw new AccessViolationException("Connection string not found."); 

            blobServiceClient = new BlobServiceClient(connectionString);
            if (blobServiceClient is null) throw new MemberAccessException("Blob Services not found.");

            blobContainerClient = new BlobContainerClient(connectionString, Environment.GetEnvironmentVariable("CONTAINER_NAME"));
            if (blobContainerClient is null) throw new MemberAccessException("Blob Container Services not found.");
            
            baseBlobUri = Environment.GetEnvironmentVariable("BASE_BLOB_URI");
        }
#endif

        public string GetMostRecentResume()
        {
            try
            {
                return string.Concat(baseBlobUri, blobContainerClient.GetBlobs().OrderBy(e => e.Properties.LastModified).LastOrDefault()?.Name);
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.Message);
            }
        }
    }
}
