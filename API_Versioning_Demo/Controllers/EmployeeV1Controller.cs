using Microsoft.AspNetCore.Mvc;

namespace API_Versioning_Demo.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/[controller]")]
    public class EmployeeV1Controller : ControllerBase
    { 
        [HttpGet]
        [Route("employees")]
        public IActionResult Get()
        {
            return new OkObjectResult("employees from V1 controller");
        }
    }
}
