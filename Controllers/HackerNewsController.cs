using Microsoft.AspNetCore.Mvc;
using HackerNewsStories.Services;
using HackerNewsStories.Models;

namespace HackerNewsBestStories.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HackerNewsController : ControllerBase
    {
        private readonly IHackerNewsService _hackerNewsService;

        public HackerNewsController(IHackerNewsService hackerNewsService)
        {
            _hackerNewsService = hackerNewsService;
        }

        // GET /api/hackernews/beststories?count=10
        [HttpGet("beststories")]
        public async Task<ActionResult<IEnumerable<HackerNewsStory>>> GetBestStories([FromQuery] int count = 10)
        {
            if (count <= 0)
                return BadRequest("Count must be greater than zero.");

            var stories = await _hackerNewsService.GetBestStoriesAsync(count);
            return Ok(stories);
        }
    }
}