using AspireAppNeon.ApiService.Data;
using AspireAppNeon.ApiService.Models;
using AspireAppNeon.ApiService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

var env = builder.Environment.EnvironmentName;

var connectionString = builder.Configuration.GetConnectionString(env);

// Add Database
builder.Services.AddDbContext<BlogDbContext>(options => options.UseNpgsql(connectionString));


// Add services to the container.
builder.Services.AddProblemDetails();

// Add Services
builder.Services.AddScoped<IBlogService, BlogService>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "OoenAPI V1");
    });

    app.UseReDoc(options =>
    {
        options.SpecUrl("/openapi/v1.json");
    });

    app.MapScalarApiReference();

    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<BlogDbContext>();
    await context.Database.EnsureCreatedAsync().ConfigureAwait(true);
}

// Map Endpoints
app.MapGet("/api/blog", async (IBlogService blogService) =>
    await blogService.GetAllPostsAsync());

app.MapGet("/api/blog/{id:int}", async (int id, IBlogService blogService) =>
{
    var post = await blogService.GetPostByIdAsync(id);
    return post is null ? Results.NotFound() : Results.Ok(post);
});

app.MapPost("/api/blog", async (BlogPost post, IBlogService blogService) =>
{
    var createdPost = await blogService.AddPostAsync(post);
    return Results.Created($"/api/blog/{createdPost.Id}", createdPost);
});

app.MapDelete("/api/blog/{id:int}", async (int id, IBlogService blogService) =>
{
    return await blogService.DeletePostAsync(id) ? Results.NoContent() : Results.NotFound();
});

app.MapDefaultEndpoints();

app.Run();
