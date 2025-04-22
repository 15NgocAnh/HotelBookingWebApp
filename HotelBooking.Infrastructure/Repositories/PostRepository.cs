using AutoMapper;
using HotelBooking.Data;
using HotelBooking.Data.Models;
using HotelBooking.Domain.DTOs.Post;
using HotelBooking.Domain.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Domain.Repository
{
    public class PostRepository : GenericRepository<PostModel>, IPostRepository
    {
        public PostRepository(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<PostModel> AddPostAsync(UserModel user, PostModel post)
        {
            if (user.Posts == null)
                user.Posts = new List<PostModel>();
            user.Posts.Add(post);
            await _context.SaveChangesAsync();
            return post;
        }

        public async Task<PostModel> UpdateAsync(int id, PostDTO postDTO)
        {
            var post = _context.Posts.FirstOrDefault(p => p.Id == id);
            if (post != null)
            {
                post = _mapper.Map(postDTO, post);
                await _context.SaveChangesAsync();
            }
            return post;
        }

        public async Task DeleteAsync(int postId)
        {
            var post = _context.Posts.Where(p => p.IsDeleted == false).FirstOrDefault(p => p.Id == postId);
            if (post != null)
            {
                post.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task ActiveAsync(int postId)
        {
            var post = _context.Posts.Where(p => p.IsDeleted == false && p.IsActived == false).FirstOrDefault(p => p.Id == postId);
            if (post != null)
            {
                post.IsActived = true;
                await _context.SaveChangesAsync();
            }
        }

        public IQueryable<PostModel> GetUserPosts(int userId)
        {
            return _context.Posts.Where(p => p.IsDeleted == false)
                .Where(c => c.User.Id == userId);
        }

        public IQueryable<PostModel> GetPosts()
        {
            return _context.Posts.Where(p => p.IsDeleted == false);
        }

        public IQueryable<PostModel> GetPostNotDeleted()
        {
            return _context.Posts.Where(p => p.IsDeleted == false);
        }

        public IQueryable<PostModel> GetPostNotApproved()
        {
            return _context.Posts.Where(p => p.IsDeleted == false)
                .Where(p => p.IsActived == false);
        }

        public async Task AddJobToPostAsync(int jobId, PostModel post)
        {
            post.JobId = jobId;
            await _context.SaveChangesAsync();
        }

        public PostModel GetPostById(int id)
        {
            return _context.Posts.IgnoreQueryFilters()
                .Where(p => p.Id == id)
                .FirstOrDefault();
        }

        public async Task UndoDeletedAsync(PostModel post)
        {
            post.IsDeleted = false;
            await _context.SaveChangesAsync();
        }
    }
}
