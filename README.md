# Sticky Tunes

StickyTunes is an anonymous Spotify song-sharing platform that allows users to post Spotify track URLs and share comments on songs.

## Setup

### Prerequisites

- .NET SDK 8.0+
- MySQL
- Spotify Developer Account

### Configuration

1. Clone the repository:

    ```bash
    git clone https://github.com/fatihyd/stickytunes-backend.git
    cd stickytunes-backend
    ```

2. Update `appsettings.json`:

    ```json
    {
      "ConnectionStrings": {
        "DefaultConnection": "Server=your-server;Database=stickytunes;User=dbadmin;Password=your-password;"
      },
      "Spotify": {
        "ClientId": "your-client-id",
        "ClientSecret": "your-client-secret"
      },
      "AllowedHosts": "*"
    }
    ```

3. Run migrations and update the database:

    ```bash
    dotnet ef database update
    ```

4. Run the project:

    ```bash
    dotnet run
    ```

## Endpoints

### Posts

- `GET /api/posts` - Get all posts.
- `GET /api/posts/{id}` - Get post by ID.
- `POST /api/posts` - Create a new post with a Spotify song.
- `DELETE /api/posts/{id}` - Delete a post and associated comments.

### Comments

- `GET /api/posts/{postId}/comments` - Get all comments for a post.
- `GET /api/posts/{postId}/comments/{commentId}` - Get a comment by ID.
- `POST /api/posts/{postId}/comments` - Add a comment with a Spotify song.
- `DELETE /api/posts/{postId}/comments/{commentId}` - Delete a comment.

## Architecture

This project follows an N-Layer Architecture, which typically includes the following layers:

1. **Presentation Layer**: This is responsible for handling the API.
2. **Business Logic Layer**: Contains the business logic of the application.
3. **Data Access Layer**: Manages data persistence and retrieval.

## Contact

Feel free to reach out for any questions or suggestions. You can contact me at [fa.yildiz99@gmail.com](mailto:fa.yildiz99@gmail.com).
