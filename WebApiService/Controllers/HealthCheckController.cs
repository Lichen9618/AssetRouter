using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;


namespace WebApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet]
        [EnableCors]
        public IActionResult Get()
        {
            return NoContent();
        }

        [HttpPost("{id}")]
        [EnableCors]
        public string Get(int id)
        {
            return id.ToString();
        }
    }
}
