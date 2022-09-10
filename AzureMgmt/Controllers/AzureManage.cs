using AzureMgmt.AzureService;
using Microsoft.AspNetCore.Mvc;

namespace AzureMgmt.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AzureManage :ControllerBase
    {
        public IAzureService azureService;

        public AzureManage(IAzureService azureService)
        {
            this.azureService = azureService;
        }

        [HttpGet]
        public async Task<string> GetContainerData()
        {
            DateTimeOffset? lastModifiedFileTime = DateTime.MinValue;
            string url = string.Empty;
            var res = await azureService.GetBlobs();

            foreach(var blob in res)
            {
                if (blob.Properties.LastModified > lastModifiedFileTime)
                {
                    url = $"https://ggpf.blob.core.windows.net/publicfilesggreenleaf/{blob.Name}";
                    lastModifiedFileTime = blob.Properties.LastModified;
                }
            }

            return url;
        }
    }
}
