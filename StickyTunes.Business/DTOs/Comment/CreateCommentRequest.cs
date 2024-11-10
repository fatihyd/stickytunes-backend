namespace StickyTunes.Business.DTOs;

public class CreateCommentRequest
{
    public string SpotifyUrl { get; set; }
    public string? Text { get; set; }
}