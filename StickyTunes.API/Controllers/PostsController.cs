using Microsoft.AspNetCore.Mvc;
using StickyTunes.Business.DTOs;
using StickyTunes.Business.Services;

namespace StickyTunes.API.Controllers;

[ApiController]
[Route("api/posts")]
public class PostsController : ControllerBase
{
    private readonly PostService _postService;

    public PostsController(PostService postService)
    {
        _postService = postService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var posts = await _postService.GetAllAsync();
        
        return Ok(posts);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var post = await _postService.GetByIdAsync(id);
        
        if (post == null)
            return NotFound();
        
        return Ok(post);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePostRequest postRequest)
    {
        var post = await _postService.CreateAsync(postRequest);
        
        return CreatedAtAction(nameof(GetById), new { id = post.Id}, post);
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        bool isDeleted = await _postService.DeleteAsync(id);
        
        if (!isDeleted)
            return NotFound();

        return NoContent();
    }
}