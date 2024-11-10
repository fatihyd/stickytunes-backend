using StickyTunes.Data.Models;

namespace StickyTunes.Business.DTOs;

public class GetTrackResponse
{
    public int Id { get; set; }
    public string SpotifyTrackId { get; set; }
    public string Name { get; set; }
    public string AlbumName { get; set; }
    public List<GetArtistResponse> Artists { get; set; } = new List<GetArtistResponse>();
}