# HackerNewsStories

This ASP.NET Core Web API retrieves the first **n** "best stories" from the Hacker News API, sorts them by score, and returns them in the specified JSON format. It also uses local caching to avoid spamming the Hacker News API on repeated requests.

## Requirements
- .NET 6 SDK (or higher)
- An internet connection (to reach Hacker News endpoints)

## How to Build & Run

1. Clone the repository:
   ```bash
   git clone https://github.com/pkwd/HackerNewsStories.git
2. Navigate to the project directory:
   ```bash
   cd HackerNewsStories
3. Build the project:
   ```bash
   dotnet build
4. Run the application:
   ```bash
   dotnet run
5. The API will start listening on http://localhost:5000/ by default (or https://localhost:7000 for HTTPS).

## Usage
- Endpoint: GET /api/hackernews/beststories?count={n}
- Parameters:
count (optional, default = 10) – the number of top stories to retrieve.
- Example:
   ```bash
   curl "http://localhost:5000/api/hackernews/beststories?count=5"

## Returned JSON
Example structure:

  ```json
  [
    {
      "title": "Example Title",
      "uri": "http://example.com",
      "postedBy": "username",
      "unixTime": 1675828673,
      "score": 123,
      "commentCount": 45,
      "time": "2023-02-07T10:17:53+00:00"
    },
    ...
  ]
```
**Note:** The time property is derived from unixTime to match the required date format.

## Assumptions
- We assume that the Hacker News service is reliable for our requests. In case of large n, we cache story metadata to prevent repeated requests.
- The default cache expiration for both story IDs and story details is 5 minutes (configurable in HackerNewsService.cs).
## Handling Large Request Volumes
- The in-memory caching ensures subsequent requests for the same set of stories do not re-fetch data, thus reducing calls to Hacker News.
- If the load is extremely high or the application is scaled out, an external distributed cache (e.g., Redis) might be preferable.
## Potential Enhancements
- Pagination: If n can be very large, returning smaller pages might improve performance.
- Distributed Caching: For multi-instance deployments, use a distributed cache to keep caches in sync.
- Security: Add authentication/authorization if the API needs securing.
- Automated Testing: Integrate a test project with unit and integration tests.
