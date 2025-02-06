using AspireAppNeon.ApiService.Models;

namespace AspireAppNeon.ApiService.Services;

public interface IBlogService
{
    Task<IEnumerable<BlogPost>> GetAllPostsAsync();
    Task<BlogPost?> GetPostByIdAsync(int id);
    Task<BlogPost> AddPostAsync(BlogPost post);
    Task<bool> DeletePostAsync(int id);
}