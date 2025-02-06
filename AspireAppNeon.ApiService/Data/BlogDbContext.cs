using AspireAppNeon.ApiService.Models;
using Microsoft.EntityFrameworkCore;

namespace AspireAppNeon.ApiService.Data;

public class BlogDbContext(DbContextOptions<BlogDbContext> options) : DbContext(options)
{
    public DbSet<BlogPost> BlogPosts { get; set; } = null!;
}