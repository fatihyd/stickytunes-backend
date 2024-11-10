using StickyTunes.Data.Models;

namespace StickyTunes.Data.Repositories;

public interface ICommentRepository : IGenericRepository<Comment>
{
    public Task<IEnumerable<Comment>> GetAllByPostIdAsync(int postId);
    public Task<Comment> GetByPostAndCommentIdAsync(int postId, int commentId);
    public Task<bool> DeleteByPostAndCommentIdAsync(int postId, int commentId);
}