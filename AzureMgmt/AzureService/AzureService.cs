using Azure.Storage.Blobs;
using AzureMgmt.Util;

namespace AzureMgmt.AzureService
{
    public class AzureService : IAzureService
    {
        private readonly string? CONNECTION_STRING;
        private readonly BlobServiceClient blobServiceClient;
        private readonly BlobContainerClient blobContainerClient;
        private readonly string? baseBlobUri;

#if DEBUG
        IJSONUtils jsonUtils;
        public AzureService(IJSONUtils utils)
        {
            jsonUtils = utils;

            CONNECTION_STRING = jsonUtils.GetJSONItems().AzureConnectionString;

            if (string.IsNullOrEmpty(CONNECTION_STRING)) throw new AccessViolationException("Connection string not found.");

            blobServiceClient = new BlobServiceClient(CONNECTION_STRING);
            if (blobServiceClient is null) throw new MemberAccessException("Blob Services not found.");

            blobContainerClient = new BlobContainerClient(CONNECTION_STRING, jsonUtils.GetJSONItems().ContainerName);
            if (blobContainerClient is null) throw new MemberAccessException("Blob Container Services not found.");
            
            baseBlobUri = jsonUtils.GetJSONItems().BlobBaseURI;
            if (string.IsNullOrEmpty(baseBlobUri)) throw new MemberAccessException("Blob Base URI Not Found.");
        }
#else
        public AzureService()
        {
            CONNECTION_STRING = Environment.GetEnvironmentVariable("CONNECTION_STRING") ?? String.Empty; 
            if (string.IsNullOrEmpty(CONNECTION_STRING)) throw new AccessViolationException("Connection string not found."); 

            blobServiceClient = new BlobServiceClient(CONNECTION_STRING);
            if (blobServiceClient is null) throw new MemberAccessException("Blob Services not found.");

            blobContainerClient = new BlobContainerClient(CONNECTION_STRING, Environment.GetEnvironmentVariable("CONTAINER_NAME"));
            if (blobContainerClient is null) throw new MemberAccessException("Blob Container Services not found.");
            
            baseBlobUri = Environment.GetEnvironmentVariable("BASE_BLOB_URI");
        }
#endif

        public string GetMostRecentResume()
        {
            try
            {
                return string.Concat(baseBlobUri, blobContainerClient.GetBlobs().OrderBy(e => e.Properties.LastModified).Last().Name);
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.Message);
            }
        }
    }
}
