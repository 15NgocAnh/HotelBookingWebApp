using HotelBooking.Data.Models;
using HotelBooking.Domain.DTOs.Post;
namespace HotelBooking.Domain.Repository.Interfaces
{
    public interface IPostRepository : IGenericRepository<PostModel>
    {
        IQueryable<PostModel> GetUserPosts(int userId);
        IQueryable<PostModel> GetPosts();
        PostModel GetPostById(int id);
        Task<PostModel> AddPostAsync(UserModel userpost, PostModel post);
        Task<PostModel> UpdateAsync(int post_id, PostDTO post);
        Task DeleteAsync(int post_id);
        Task ActiveAsync(int post_id);
        Task addJobToPostAsync(int jobId, PostModel post);
        Task UndoDeletedAsync(PostModel post);
    }
}
