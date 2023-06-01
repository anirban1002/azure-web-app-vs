using azure_web_api.Data;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace azure_web_api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EngineerController : ControllerBase
    {
        IEngineerService _engineerService;
        public EngineerController(IEngineerService engineerService)
        {
            _engineerService = engineerService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Engineer>>> GetEngineer()
        {
            return Ok(await _engineerService.GetEngineerDetails());
        }

        [HttpGet]
        public async Task<ActionResult<Engineer>> GetEngineerById(string id, string partitionKey)
        {
            return Ok(await _engineerService.GetEngineerDetailsById(id, partitionKey));
        }

        [HttpPost]
        public async Task<ActionResult<string>> AddEngineer([FromBody] Engineer engineer)
        {
            return Ok(await _engineerService.AddEngineer(engineer));
        }

        [HttpPut]
        public async Task<ActionResult<string>> UpdateEngineer([FromBody] Engineer engineer)
        {
            return Ok(await _engineerService.UpdateEngineer(engineer));
        }

        [HttpDelete]
        public async Task<ActionResult<string>> DeleteEngineer(string id, string partitionKey)
        {
            return Ok(await _engineerService.DeleteEngineer(id, partitionKey));
        }
    }
}
