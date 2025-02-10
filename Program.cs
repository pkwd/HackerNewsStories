using HackerNewsStories.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<HttpClient>();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IHackerNewsService, HackerNewsService>();

var app = builder.Build();

app.MapControllers();
app.Run();