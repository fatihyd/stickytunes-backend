using StickyTunes.Business.DTOs;
using StickyTunes.Data.Models;
using StickyTunes.Data.Repositories;

namespace StickyTunes.Business.Services;

public class PostService
{
    private readonly IPostRepository _postRepository;
    private readonly ITrackRepository _trackRepository;
    private readonly SpotifyService _spotifyService;

    public PostService(IPostRepository postRepository, ITrackRepository trackRepository, SpotifyService spotifyService)
    {
        _postRepository = postRepository;
        _trackRepository = trackRepository;
        _spotifyService = spotifyService;
    }

    public async Task<List<GetPostResponse>> GetAllAsync()
    {
        var posts = await _postRepository.GetAllAsync();

        var postResponses = posts.Select(post => new GetPostResponse
        {
            Id = post.Id,
            Track = new GetTrackResponse
            {
                Id = post.Track.Id,
                SpotifyTrackId = post.Track.SpotifyTrackId,
                Name = post.Track.Name,
                AlbumName = post.Track.AlbumName,
                Artists = post.Track.Artists.Select(artist => new GetArtistResponse
                {
                    Name = artist.Name
                }).ToList()
            },
            Text = post.Text,
            DatePosted = post.DatePosted
        }).ToList();
        return postResponses;
    }
    
    public async Task<GetPostResponse> GetByIdAsync(int id)
    {
        var post = await _postRepository.GetByIdAsync(id);
        
        if (post == null)
            return null;

        var postResponse = new GetPostResponse
        {
            Id = post.Id,
            Track = new GetTrackResponse
            {
                Id = post.Track.Id,
                SpotifyTrackId = post.Track.SpotifyTrackId,
                Name = post.Track.Name,
                AlbumName = post.Track.AlbumName,
                Artists = post.Track.Artists.Select(artist => new GetArtistResponse
                {
                    Name = artist.Name
                }).ToList()
            },
            Text = post.Text,
            DatePosted = post.DatePosted
        };

        return postResponse;
    }
    
    public async Task<GetPostResponse> CreateAsync(CreatePostRequest postRequest)
    {
        var track = await _spotifyService.GetTrackAsync(postRequest.SpotifyUrl);
        await _trackRepository.CreateAsync(track);

        var post = new Post
        {
            Track = track,
            Text = postRequest.Text,
            DatePosted = DateTime.Now
        };
        await _postRepository.CreateAsync(post);

        var postResponse = new GetPostResponse
        {
            Id = post.Id,
            Track = new GetTrackResponse
            {
                Id = post.Track.Id,
                SpotifyTrackId = post.Track.SpotifyTrackId,
                Name = post.Track.Name,
                AlbumName = post.Track.AlbumName,
                Artists = post.Track.Artists.Select(artist => new GetArtistResponse
                {
                    Name = artist.Name
                }).ToList()
            },
            Text = post.Text,
            DatePosted = post.DatePosted
        };
        
        return postResponse;
    }
    
    public async Task<bool> DeleteAsync(int id)
    {
        bool isDeleted = await _postRepository.DeleteAsync(id);

        return isDeleted;
    }
}