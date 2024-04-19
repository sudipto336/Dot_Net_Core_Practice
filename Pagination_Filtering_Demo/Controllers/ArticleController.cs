using Microsoft.AspNetCore.Mvc;
using Pagination_Filtering_Demo.Models;

namespace Pagination_Filtering_Demo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticleController : ControllerBase
    {
        private static readonly List<Article> _articles = new List<Article>()
        {
            new Article() { ArticleId = 1,  ArticleTitle = "First Article", ArticleCategory = "Comics"},
            new Article() { ArticleId = 2,  ArticleTitle = "Second Article", ArticleCategory = "Detective" },
            new Article() { ArticleId = 3,  ArticleTitle = "Third Article", ArticleCategory = "Biography"},
            new Article() { ArticleId = 4,  ArticleTitle = "Fourth Article", ArticleCategory = "Detective"},
            new Article() { ArticleId = 5,  ArticleTitle = "Fifth Article", ArticleCategory = "Biography"},
            new Article() { ArticleId = 6,  ArticleTitle = "Sixth Article", ArticleCategory = "Comics"},
            new Article() { ArticleId = 7,  ArticleTitle = "Seventh Article", ArticleCategory = "Comics"},
            new Article() { ArticleId = 8,  ArticleTitle = "Eighth Article", ArticleCategory = "Detective"},
            new Article() { ArticleId = 9,  ArticleTitle = "Nineth Article", ArticleCategory = "Biography"},
            new Article() { ArticleId = 10,  ArticleTitle = "Tenth Article", ArticleCategory = "Comics"},
        };

        private readonly ILogger<ArticleController> _logger;

        public ArticleController(ILogger<ArticleController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetArticles")]
        [Route("GetArticles")]
        public IActionResult Get([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string filter = "")
        {
            var query = _articles.AsQueryable();

            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(article => article.ArticleTitle.Contains(filter) || article.ArticleCategory.Contains(filter));
            }

            var totalCount = query.Count();

            var totalPages = (int)Math.Ceiling((double)totalCount/pageSize);

            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            var result = new
            {
                TotalCount = totalCount,
                TotalPages = totalPages,
                CurrentPage = page,
                pageSize = pageSize,
                Artilces = query.ToList()
            };

            return Ok(result);

        }
    }
}
