# Sticky Tunes

## N-Layer Architecture ASP.NET Core Project

StickyTunes is an anonymous Spotify song-sharing platform that allows users to post Spotify track URLs and share comments on songs.

## Table of Contents

- [Sticky Tunes](#sticky-tunes)
- [Table of Contents](#table-of-contents)
- [Getting Started](#getting-started)
    - [Prerequisites](#prerequisites)
    - [Installation](#installation)
    - [Database Setup](#database-setup)
- [Usage](#usage)
- [Architecture](#architecture)
- [Contributing](#contributing)
- [License](#license)
- [Contact](#contact)

## Getting Started

### Prerequisites

Make sure you have the following installed:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [MySQL](https://dev.mysql.com/downloads/mysql/) and [MySQL Workbench](https://dev.mysql.com/downloads/workbench/) (optional)

### Installation

1. Clone the repository:

    ```bash
    git clone https://github.com/fatihyd/stickytunes-backend.git
    ```

2. Navigate to the project directory:

    ```bash
    cd your-repo-name
    ```

3. Restore dependencies:

    ```bash
    dotnet restore
    ```

4. Build the project:

    ```bash
    dotnet build
    ```

### Database Setup

1. Ensure you have [MySQL](https://dev.mysql.com/downloads/mysql/) installed and running.

2. Update the connection string in `appsettings.json`:

    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=your_server_name;Database=your_database_name;User=root;Password=your_password;"
    }
    ```

3. Install the necessary EF Core tools if not already installed:

    ```bash
    dotnet tool install --global dotnet-ef
    ```

4. Apply any pending migrations to the database:

    ```bash
    dotnet ef database update
    ```

## Usage

1. Run the project:

    ```bash
    dotnet run
    ```

2. Check the URLs defined in the `Properties/launchSettings.json` file to open your browser and navigate to the appropriate URL. By default, these are often `https://localhost:5001` or `http://localhost:5000`.

## Architecture

This project follows an N-Layer Architecture, which typically includes the following layers:

1. **Presentation Layer**: This is responsible for handling the UI and user interactions.
2. **Business Logic Layer**: Contains the business logic of the application, also known as the service layer.
3. **Data Access Layer**: Manages data persistence and retrieval.
4. **Common Layer** (optional): Includes any common utilities or classes that are shared across layers.

## Contact

Feel free to reach out for any questions or suggestions. You can contact me at [fa.yildiz99@gmail.com](mailto:fa.yildiz99@gmail.com).
