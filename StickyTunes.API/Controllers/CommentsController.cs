using Microsoft.AspNetCore.Mvc;
using StickyTunes.Business.DTOs;
using StickyTunes.Business.Services;

namespace StickyTunes.API.Controllers;

[ApiController]
[Route("api/posts/{postId}/comments")]
public class CommentsController : ControllerBase
{
    private readonly CommentService _commentService;

    public CommentsController(CommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllByPostId([FromRoute] int postId)
    {
        var comments = await _commentService.GetAllByPostIdAsync(postId);
        
        return Ok(comments);
    }

    [HttpGet]
    [Route("{commentId}")]
    public async Task<IActionResult> GetByPostAndCommentId([FromRoute] int postId, [FromRoute] int commentId)
    {
        var comment = await _commentService.GetByPostAndCommentIdAsync(postId, commentId);

        if (comment == null)
            return NotFound();

        return Ok(comment);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromRoute] int postId, [FromBody] CreateCommentRequest commentRequest)
    {
        var comment = await _commentService.CreateAsync(postId, commentRequest);
        
        return CreatedAtAction(nameof(GetByPostAndCommentId), new { postId = postId, commentId = comment.Id}, comment);
    }

    [HttpDelete]
    [Route("{commentId}")]
    public async Task<IActionResult> DeleteByPostAndCommentId([FromRoute] int postId, [FromRoute] int commentId)
    {
        bool isDeleted = await _commentService.DeleteByPostAndCommentIdAsync(postId, commentId);
        
        if (!isDeleted)
            return NotFound();

        return NoContent();
    }
}