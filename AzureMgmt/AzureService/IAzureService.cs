using Azure.Storage.Blobs.Models;

namespace AzureMgmt.AzureService
{
    public interface IAzureService
    {
         Task<List<BlobItem>> GetBlobs();

    }
}
