using Azure.Storage.Blobs.Models;

namespace AzureMgmt.AzureService
{
    public interface IAzureService
    {
        string GetMostRecentResume();
    }
}
