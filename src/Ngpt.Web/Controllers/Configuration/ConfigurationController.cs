using Augur.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Ngpt.Web.Controllers.Configuration
{
    public class ConfigurationController : AugurApiController
    {
        public readonly string clientId;
        public ConfigurationController(IConfiguration configuration)
        {
            this.clientId = configuration.GetSection("Google")["client_id"];
        }

        [HttpGet(nameof(GetConfig))]
        public IActionResult GetConfig()
        {
            return Ok(new { ClientId = this.clientId });
        }
    }
}
