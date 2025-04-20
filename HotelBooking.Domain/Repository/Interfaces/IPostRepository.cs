using HotelBooking.Data.Models;
using HotelBooking.Domain.DTOs.Post;
namespace HotelBooking.Domain.Repository.Interfaces
{
    public interface IPostRepository : IGenericRepository<PostModel>
    {
        IQueryable<PostModel> GetUserPosts(int userId);
        IQueryable<PostModel> GetPosts();
        IQueryable<PostModel> GetPostNotDeleted();
        IQueryable<PostModel> GetPostNotApproved();
        PostModel GetPostById(int id);
        Task<PostModel> AddPostAsync(UserModel user, PostModel post);
        Task<PostModel> UpdateAsync(int id, PostDTO postDTO);
        Task DeleteAsync(int postId);
        Task ActiveAsync(int postId);
        Task AddJobToPostAsync(int jobId, PostModel post);
        Task UndoDeletedAsync(PostModel post);
    }
}
