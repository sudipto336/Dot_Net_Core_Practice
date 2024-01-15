using Calling_Remote_API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Calling_Remote_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : Controller
    {
        private IConfiguration _configuration;
        private HttpClient _client;

        public PostsController(IConfiguration configuration, HttpClient client)
        {
            _configuration = configuration;
            _client = client;
        }

        [HttpGet]
        [Route("GetPosts")]
        public async Task<IActionResult> GetTodosFromAPIUsingHttpClient()
        {
            string? JSON_PLACEHOLDER_URL = _configuration.GetValue<string>("JSON_PLaceholder_URL");
            string postsUrl = "posts";
            IEnumerable<TodoDetail> todos = new List<TodoDetail>();

            _client.BaseAddress = new Uri(String.IsNullOrEmpty(JSON_PLACEHOLDER_URL) ? "" : JSON_PLACEHOLDER_URL);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var responseTask = await _client.GetAsync(postsUrl);

            if (responseTask.IsSuccessStatusCode)
            {
                string data = await responseTask.Content.ReadAsStringAsync();
                return Ok(data);
            } else {
                return StatusCode((int)responseTask.StatusCode);
            }
        }
    }
}
