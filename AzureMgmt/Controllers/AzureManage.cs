using AzureMgmt.AzureService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

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

        [HttpGet("MostRecentResume")]
        public IActionResult MostRecentResume()
        {
            try
            {
                return Ok(azureService.GetMostRecentResume() ?? "Resume Not Found");
            }
            catch (Exception ex)
            {
                return NotFound($"Not Found: {ex.Message}");
            }
        }
    }
}
