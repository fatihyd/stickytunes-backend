using Microsoft.EntityFrameworkCore;
using StickyTunes.Data.Models;

namespace StickyTunes.Data.Contexts;

public class StickyTunesDbContext : DbContext
{
    public StickyTunesDbContext(DbContextOptions<StickyTunesDbContext> options) : base(options) { }
    
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Track> Tracks { get; set; }
    public DbSet<Artist> Artists { get; set; }
}