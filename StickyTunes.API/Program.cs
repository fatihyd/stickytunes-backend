using System.Reflection;
using System.Text;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StickyTunes.Business;
using StickyTunes.Business.Filters;
using StickyTunes.Business.Services;
using StickyTunes.Data.Contexts;
using StickyTunes.Data.Models;
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

// Identity service
// This registers the ASP.NET Core Identity framework for managing users and roles (
// -> using StickyTunesDbContext for storage,
// -> using ApiUser and ApiRole for managing user and role information
// ).
builder.Services.AddIdentity<ApiUser, ApiRole>()
    .AddEntityFrameworkStores<StickyTunesDbContext>();

// Authentication service
// This sets up the authentication mechanism for the application using JWT.
// The `JwtBearerDefaults.AuthenticationScheme` specifies that JWT Bearer tokens will be used for authentication.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // Token validation parameters specify how the server should validate incoming JWT tokens.
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // Ensures the token was issued by a trusted authority (issuer).
            ValidateAudience = true, // Ensures the token is intended for the correct audience (your API or app).
            ValidateLifetime = true, // Ensures the token hasn't expired.
            ValidateIssuerSigningKey = true, // Ensures the token was signed with the correct secret key.

            // Expected issuer for tokens.
            ValidIssuer = builder.Configuration["JWT:Issuer"], 

            // Expected audience for tokens.
            ValidAudience =  builder.Configuration["JWT:Audience"], 

            // Signing key used to validate the token's signature. This must match the key used to generate the token.
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])) 
        };
    });

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();