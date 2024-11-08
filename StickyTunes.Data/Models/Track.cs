namespace StickyTunes.Data.Models;

public class Track
{
    public int Id { get; set; }
    public string SpotifyTrackId { get; set; }
    public string Name { get; set; }
    public string AlbumName { get; set; }
    public List<Artist> Artists { get; set; } = new List<Artist>();
    public List<Post> Posts { get; set; } = new List<Post>();
    public List<Comment> Comments { get; set; } = new List<Comment>();
}