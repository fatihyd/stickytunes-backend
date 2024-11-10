using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using StickyTunes.Business.Services;
using StickyTunes.Data.Contexts;
using StickyTunes.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

/* *** WHAT I ADDED *** */
/* begin */

// Database connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<StickyTunesDbContext>(options => { options.UseMySQL(connectionString); });

// HttpClient for external API calls
builder.Services.AddHttpClient();

// bunch of stuff
builder.Services.AddScoped<SpotifyService>();
builder.Services.AddScoped<PostService>();
builder.Services.AddScoped<CommentService>();

builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ITrackRepository, TrackRepository>();

// CORS stuff
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

/* end */

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

/* *** WHAT I ADDED *** */
/* begin */

// More CORS stuff
app.UseCors("AllowAll");

/* end */

app.UseAuthorization();

app.MapControllers();

app.Run();