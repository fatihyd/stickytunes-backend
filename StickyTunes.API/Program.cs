using System.Reflection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StickyTunes.Business;
using StickyTunes.Business.Filters;
using StickyTunes.Business.Services;
using StickyTunes.Data.Contexts;
using StickyTunes.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

/* *** WHAT I ADDED *** */
/* begin */

// Suppress the default model state invalid filter
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Fluent Validation
builder.Services.AddControllers(options =>
    {
        options.Filters.Add<FluentValidationFilter>();
    })
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<BusinessLayerAssemblyMarker>());

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

builder.Services.AddScoped<FluentValidationFilter>();

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