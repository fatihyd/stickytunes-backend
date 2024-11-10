using Microsoft.EntityFrameworkCore;
using StickyTunes.Data.Contexts;
using StickyTunes.Data.Models;

namespace StickyTunes.Data.Repositories;

public class CommentRepository : GenericRepository<Comment>, ICommentRepository
{
    public CommentRepository(StickyTunesDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Comment>> GetAllByPostIdAsync(int postId)
    {
        var comments = await _context.Comments
            .Where(c => c.Post.Id == postId)
            .Include(c => c.Track)
            .ToListAsync();

        return comments;
    }

    public async Task<Comment> GetByPostAndCommentIdAsync(int postId, int commentId)
    {
        var comment = await _context.Comments
            .Where(c => c.Id == commentId && c.Post.Id == postId)
            .Include(p => p.Track)
            .FirstOrDefaultAsync();
        
        return comment;
    }

    public async Task<bool> DeleteByPostAndCommentIdAsync(int postId, int commentId)
    {
        var comment = await GetByPostAndCommentIdAsync(postId, commentId);
        
        if (comment == null)
            return false;
        
        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();

        return true;
    }
}