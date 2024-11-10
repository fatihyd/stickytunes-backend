using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using StickyTunes.Data.Models;

namespace StickyTunes.Business.Services;

public class SpotifyService
{
    private readonly HttpClient _httpClient;
    private readonly string _clientId;
    private readonly string _clientSecret;

    public SpotifyService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _clientId = config["Spotify:ClientId"];
        _clientSecret = config["Spotify:ClientSecret"];
    }

    public async Task<string> GetAccessTokenAsync()
    {
        // Endpoint for retrieving token
        var tokenEndpoint = "https://accounts.spotify.com/api/token";

        // Prepare client credentials for authorization
        var clientCredentials = $"{_clientId}:{_clientSecret}";
        var encodedCredentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(clientCredentials));
        var authorizationHeader = $"Basic {encodedCredentials}";

        // Build the HTTP request
        var requestBody = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(tokenEndpoint),
            Headers =
            {
                { "Authorization", authorizationHeader }
            },
            Content = requestBody
        };

        // Send the request and handle the response
        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to retrieve access token.");
        }

        // Parse the response to get the token
        var jsonResponse = await response.Content.ReadAsStringAsync();
        var token = JsonDocument.Parse(jsonResponse).RootElement.GetProperty("access_token").GetString();

        return token;
    }

    public string ExtractTrackId(string trackUrl)
    {
        var uri = new Uri(trackUrl);
        var segments = uri.Segments;
        if (segments.Length < 3 || segments[1].ToLower() != "track/")
        {
            // Return an empty string if it doesn't match the expected structure
            return string.Empty;
        }

        return segments.Last();
    }

    public async Task<Track> GetTrackAsync(string trackUrl)
    {
        // Extract the track id from the URL
        var trackId = ExtractTrackId(trackUrl);

        // Get the access token first
        var accessToken = await GetAccessTokenAsync();

        // Spotify API endpoint for fetching track details
        var trackEndpoint = $"https://api.spotify.com/v1/tracks/{trackId}";

        // Build the HTTP request
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(trackEndpoint),
            Headers =
            {
                { "Authorization", $"Bearer {accessToken}" }
            }
        };

        // Send the request and handle the response
        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Failed to retrieve track data: {response.StatusCode}");
        }

        // Parse the response JSON
        var jsonResponse = await response.Content.ReadAsStringAsync();
        var jsonDocument = JsonDocument.Parse(jsonResponse);
        
        // Map relevant fields from the JSON to the Track object
        var track = new Track
        {
            SpotifyTrackId = jsonDocument.RootElement.GetProperty("id").GetString(),
            Name = jsonDocument.RootElement.GetProperty("name").GetString(),
            AlbumName = jsonDocument.RootElement.GetProperty("album").GetProperty("name").GetString(),
            Artists = jsonDocument.RootElement.GetProperty("artists").EnumerateArray()
                .Select(artist => new Artist 
                { 
                    Name = artist.GetProperty("name").GetString() 
                }).ToList()
        };

        return track;
    }
}