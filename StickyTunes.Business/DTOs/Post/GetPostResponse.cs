using StickyTunes.Data.Models;

namespace StickyTunes.Business.DTOs;

public class GetPostResponse
{
    public int Id { get; set; }
    public GetTrackResponse Track { get; set; }
    public string Text { get; set; }
    public DateTime DatePosted { get; set; }
}