using StickyTunes.Data.Contexts;
using StickyTunes.Data.Models;

namespace StickyTunes.Data.Repositories;

public class TrackRepository : GenericRepository<Track>, ITrackRepository
{
    public TrackRepository(StickyTunesDbContext context) : base(context)
    {
    }
}