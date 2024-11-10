using Microsoft.EntityFrameworkCore;
using StickyTunes.Data.Contexts;
using StickyTunes.Data.Models;

namespace StickyTunes.Data.Repositories;

public class PostRepository : GenericRepository<Post>, IPostRepository
{
    public PostRepository(StickyTunesDbContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<Post>> GetAllAsync()
    {
        return await _context.Posts
            .Include(p => p.Track)
            .ToListAsync();
    }

    public override async Task<Post> GetByIdAsync(int id)
    {
        return await _context.Posts
            .Include(p => p.Track)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
}