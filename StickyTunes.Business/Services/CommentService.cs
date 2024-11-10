using StickyTunes.Business.DTOs;
using StickyTunes.Data.Models;
using StickyTunes.Data.Repositories;

namespace StickyTunes.Business.Services;

public class CommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly ITrackRepository _trackRepository;
    private readonly IPostRepository _postRepository;
    private readonly SpotifyService _spotifyService;

    public CommentService(ICommentRepository commentRepository, ITrackRepository trackRepository, IPostRepository postRepository, SpotifyService spotifyService)
    {
        _commentRepository = commentRepository;
        _trackRepository = trackRepository;
        _postRepository = postRepository;
        _spotifyService = spotifyService;
    }

    public async Task<List<GetCommentResponse>> GetAllByPostIdAsync(int postId)
    {
        var comments = await _commentRepository.GetAllByPostIdAsync(postId);
        
        var commentResponses = comments.Select(comment => new GetCommentResponse
        {
            Id = comment.Id,
            Track = new GetTrackResponse
            {
                Id = comment.Track.Id,
                SpotifyTrackId = comment.Track.SpotifyTrackId,
                Name = comment.Track.Name,
                AlbumName = comment.Track.AlbumName,
                Artists = comment.Track.Artists.Select(artist => new GetArtistResponse
                {
                    Name = artist.Name
                }).ToList()
            },

            Text = comment.Text,
            DatePosted = comment.DatePosted
        }).ToList();

        return commentResponses;
    }
    
    public async Task<GetCommentResponse> GetByPostAndCommentIdAsync(int postId, int commentId)
    {
        var comment = await _commentRepository.GetByPostAndCommentIdAsync(postId, commentId);

        if (comment == null)
            return null;

        var commentResponse = new GetCommentResponse
        {
            Id = comment.Id,
            Track = new GetTrackResponse
            {
                Id = comment.Track.Id,
                SpotifyTrackId = comment.Track.SpotifyTrackId,
                Name = comment.Track.Name,
                AlbumName = comment.Track.AlbumName,
                Artists = comment.Track.Artists.Select(artist => new GetArtistResponse
                {
                    Name = artist.Name
                }).ToList()
            },

            Text = comment.Text,
            DatePosted = comment.DatePosted
        };

        return commentResponse;
    }

    public async Task<GetCommentResponse> CreateAsync(int postId, CreateCommentRequest commentRequest)
    {
        var track = await _spotifyService.GetTrackAsync(commentRequest.SpotifyUrl);
        await _trackRepository.CreateAsync(track);

        var comment = new Comment
        {
            Track = track,
            Text = commentRequest.Text,
            DatePosted = DateTime.Now,
            Post = await _postRepository.GetByIdAsync(postId)
        };
        await _commentRepository.CreateAsync(comment);

        var commentResponse = new GetCommentResponse
        {
            Id = comment.Id,
            Track = new GetTrackResponse
            {
                Id = comment.Track.Id,
                SpotifyTrackId = comment.Track.SpotifyTrackId,
                Name = comment.Track.Name,
                AlbumName = comment.Track.AlbumName,
                Artists = comment.Track.Artists.Select(artist => new GetArtistResponse
                {
                    Name = artist.Name
                }).ToList()
            },

            Text = comment.Text,
            DatePosted = comment.DatePosted
        };
        
        return commentResponse;
    }
    
    public async Task<bool> DeleteByPostAndCommentIdAsync(int postId, int commentId)
    {
        bool isDeleted = await _commentRepository.DeleteByPostAndCommentIdAsync(postId, commentId);

        return isDeleted;
    }
}