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

        [HttpGet("MostRecentResume")]
        public IActionResult MostRecentResume()
        {
            try
            {
                var linkPostFix = azureService.GetMostRecentResume();
                
                return linkPostFix is not null ? Ok(linkPostFix) : NotFound("Resume Not Found");
            }
            catch (Exception ex)
            {
                return NotFound($"Not Found: {ex.Message}");
            }
        }
    }
}
