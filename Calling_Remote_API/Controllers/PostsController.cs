using Calling_Remote_API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Calling_Remote_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : Controller
    {
        private IConfiguration _configuration;
        private HttpClient _client;
        private readonly ILogger<PostsController> _logger;

        public PostsController(IConfiguration configuration, HttpClient client, ILogger<PostsController> logger)
        {
            _configuration = configuration;
            _client = client;
            _logger = logger;
        }

        [HttpGet]
        [Route("GetPosts")]
        public async Task<IActionResult> GetPostsFromAPI()
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

        [HttpGet]
        [Route("GetPostById/{id}")]
        public async Task<IActionResult> GetPostsByIdFromAPI(int id)
        {
            string? JSON_PLACEHOLDER_URL = _configuration.GetValue<string>("JSON_PLaceholder_URL");
            string postsUrl = $"posts/{id}";
            IEnumerable<TodoDetail> todos = new List<TodoDetail>();

            _client.BaseAddress = new Uri(String.IsNullOrEmpty(JSON_PLACEHOLDER_URL) ? "" : JSON_PLACEHOLDER_URL);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var responseTask = await _client.GetAsync(postsUrl);

            if (responseTask.IsSuccessStatusCode)
            {
                string data = await responseTask.Content.ReadAsStringAsync();
                PostDetail post = JsonSerializer.Deserialize<PostDetail>(data);
                return Ok(post);
            }
            else
            {
                return StatusCode((int)responseTask.StatusCode);
            }
        }

        [HttpPost]
        [Route("AddPost")]
        public async Task<IActionResult> AddPostToAPI([FromBody] PostDetail post)
        {
            string? JSON_PLACEHOLDER_URL = _configuration.GetValue<string>("JSON_PLaceholder_URL");
            string postUrl = $"posts";

            _client.BaseAddress = new Uri(String.IsNullOrEmpty(JSON_PLACEHOLDER_URL) ? "" : JSON_PLACEHOLDER_URL);

            StringContent postStr = new StringContent(JsonSerializer.Serialize(post), Encoding.UTF8, "application/json");

            //var responseTask = await _client.PostAsJsonAsync(postUrl, post);
            var responseTask = await _client.PostAsync(postUrl, postStr);

            if (responseTask.IsSuccessStatusCode)
            {
                return Created();
            }
            else
            {
                return StatusCode((int)responseTask.StatusCode);
            }
        }

        [HttpPut]
        [Route("UpdatePost/{id}")]
        public async Task<IActionResult> UpdatePostToAPI([FromRoute] int id, [FromBody] PostDetail post)
        {
            string? JSON_PLACEHOLDER_URL = _configuration.GetValue<string>("JSON_PLaceholder_URL");
            string putUrl = string.Format("posts/{0}", id);

            _client.BaseAddress = new Uri(String.IsNullOrEmpty(JSON_PLACEHOLDER_URL) ? "" : JSON_PLACEHOLDER_URL);

            var responseTask = await _client.PutAsJsonAsync(putUrl, post);

            if (responseTask.IsSuccessStatusCode)
            {
                return Ok();
            }
            else
            {
                return StatusCode((int)responseTask.StatusCode);
            }
        }

        [HttpDelete]
        [Route("DeletePost/{id}")]
        public async Task<IActionResult> DeletePostFromAPI([FromRoute] int id)
        {
            string? JSON_PLACEHOLDER_URL = _configuration.GetValue<string>("JSON_PLaceholder_URL");
            string deleteUrl = string.Format("posts/{0}", id);

            _client.BaseAddress = new Uri(String.IsNullOrEmpty(JSON_PLACEHOLDER_URL) ? "" : JSON_PLACEHOLDER_URL);

            var responseTask = await _client.DeleteAsync(deleteUrl);

            if (responseTask.IsSuccessStatusCode)
            {
                return Ok();
            }
            else
            {
                return StatusCode((int)responseTask.StatusCode);
            }
        }
    }
}
