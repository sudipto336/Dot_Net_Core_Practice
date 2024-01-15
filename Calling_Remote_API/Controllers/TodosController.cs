using Calling_Remote_API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Calling_Remote_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodosController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<TodosController> _logger;

        public TodosController (IConfiguration configuration, ILogger<TodosController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        //[HttpGet(Name = "GetTodosUsingHttpClient")]
        [HttpGet]
        [Route("GetTodos")]
        public async Task<IActionResult> GetTodosFromAPI()
        {
            string? JSON_PLACEHOLDER_URL = _configuration.GetValue<string>("JSON_PLaceholder_URL");
            string todosUrl = "todos";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(String.IsNullOrEmpty(JSON_PLACEHOLDER_URL) ? "" : JSON_PLACEHOLDER_URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var resposeTask = client.GetAsync(todosUrl);

                resposeTask.Wait();

                var response = resposeTask.Result;

                if (response.IsSuccessStatusCode)
                {
                    var readTask = response.Content.ReadAsStringAsync();
                    readTask.Wait();
                    string data = readTask.Result;
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };
                    IEnumerable<TodoDetail> todos = JsonSerializer.Deserialize<List<TodoDetail>>(data);
                    return Ok(todos);
                } else {
                    return StatusCode((int)response.StatusCode);
                }

            }
        }
    }
}
