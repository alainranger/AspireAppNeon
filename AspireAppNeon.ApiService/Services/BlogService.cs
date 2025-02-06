using AspireAppNeon.ApiService.Data;
using AspireAppNeon.ApiService.Models;
using Microsoft.EntityFrameworkCore;

namespace AspireAppNeon.ApiService.Services;

public class BlogService(BlogDbContext dbContext) : IBlogService
{
    private readonly BlogDbContext _dbContext = dbContext;

    public async Task<IEnumerable<BlogPost>> GetAllPostsAsync()
    {
        return await _dbContext.BlogPosts.ToListAsync();
    }

    public async Task<BlogPost?> GetPostByIdAsync(int id)
    {
        return await _dbContext.BlogPosts.FindAsync(id);
    }

    public async Task<BlogPost> AddPostAsync(BlogPost post)
    {
        var entity = _dbContext.BlogPosts.Add(post);
        await _dbContext.SaveChangesAsync();
        return entity.Entity;
    }

    public async Task<bool> DeletePostAsync(int id)
    {
        var post = await _dbContext.BlogPosts.FindAsync(id);
        if (post is null)
        {
            return false;
        }

        _dbContext.BlogPosts.Remove(post);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}