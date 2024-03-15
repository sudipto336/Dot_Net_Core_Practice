using Microsoft.AspNetCore.Mvc;

namespace API_Versioning_Demo.Controllers
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/{v:apiVersion}/[controller]")]
    public class EmployeeV2Controller : ControllerBase
    { 
        [HttpGet]
        [Route("employees")]
        public IActionResult Get()
        {
            return new OkObjectResult("employees from V2 controller");
        }
    }
}
