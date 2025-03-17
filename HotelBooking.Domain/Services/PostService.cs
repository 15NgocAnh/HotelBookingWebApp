using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using LinqKit;
using HotelBooking.Domain.Constant;
using HotelBooking.Domain.DTOs.Post;
using HotelBooking.Domain.Filtering;
using HotelBooking.Domain.Repository.Interfaces;
using HotelBooking.Domain.Response;
using HotelBooking.Domain.Services.Interfaces;
using HotelBooking.Data;
using Microsoft.Extensions.Logging;
using static HotelBooking.Domain.Response.EServiceResponseTypes;
using HotelBooking.Data.Models;


namespace HotelBooking.Domain.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IFilterHelper<PostValidatorDTO> _filterHelper;
        private readonly IFilterHelper<PostDetailsDTO> _filterHelper2; 
        private readonly ILogger<PostService> _logger;

        public PostService(IPostRepository postRepository, IUserRepository userRepository, IMapper mapper, IFilterHelper<PostValidatorDTO> filterHelper, ILogger<PostService> logger, IFilterHelper<PostDetailsDTO> filterHelper2) 
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _filterHelper = filterHelper;
            _logger = logger;
            _filterHelper2 = filterHelper2;
        }

        public async Task<ServiceResponse<PostDTO>> SaveAsync(int userId, PostDTO newPost)
        {
            var serviceResponse = new ServiceResponse<PostDTO>();
            try
            {
                var user = await _userRepository.findUserPostAsync(userId);
                newPost.file_url = $"{userId}/{CJConstant.POST_PATH}/{newPost.file_name}";
                var post = _mapper.Map<PostModel>(newPost);
                post = await _postRepository.AddPostAsync(user, post);
                serviceResponse.Data = _mapper.Map<PostDTO>(post);
                serviceResponse.ResponseType = EResponseType.Success;
                serviceResponse.Message = "Add post successfully";
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("Owner (User) of post not found.");
            }
            catch 
            {
                throw;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<PostDetailsDTO>> FindByIdAsync(int id)
        {
            var serviceResponse = new ServiceResponse<PostDetailsDTO>();
            try
            {
                serviceResponse.ResponseType = EResponseType.Success;
                serviceResponse.Data = await _mapper.ProjectTo<PostDetailsDTO>(_postRepository.GetPosts())
                    .AsNoTracking()
                    .FirstAsync(c => c.id == id);
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("Post and / or Owner(User) not found.");
            }
            catch { throw; }
            return serviceResponse;
        }

        public async Task<ServiceResponse<PostDetailsDTO>> UpdateAsync(int userId, int id, PostDTO postDTO)
        {
            var serviceResponse = new ServiceResponse<PostDetailsDTO>();
            try
            {
                var post = await _postRepository.GetUserPosts(userId)
                    .FirstOrDefaultAsync(c => c.id == id);
                if (post != null)
                {
                    post = await _postRepository.UpdateAsync(id, postDTO);
                    serviceResponse.ResponseType = EResponseType.Success;
                    serviceResponse.Message = "Update post successfully!";
                }
                else
                {
                    serviceResponse.ResponseType = EResponseType.NotFound;
                    serviceResponse.Message = "Post not found";
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new DbUpdateConcurrencyException(CJConstant.SOMETHING_WENT_WRONG);
            }
            catch { throw; }
            return serviceResponse;
        }

        public async Task<ServiceResponse<object>> DeleteAsync(int userId, int id)
        {
            var serviceResponse = new ServiceResponse<object>();
            try
            {
                var post = await _postRepository.GetUserPosts(userId)
                   .FirstOrDefaultAsync(c => c.id == id);
                if (post != null)
                {
                    await _postRepository.DeleteAsync(post.id);
                    serviceResponse.ResponseType = EResponseType.Success;
                    serviceResponse.Message = "Delete post successfully!";
                }
                else
                {
                    serviceResponse.ResponseType = EResponseType.NotFound;
                    serviceResponse.Message = "Post not found";
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new DbUpdateConcurrencyException(CJConstant.SOMETHING_WENT_WRONG);
            }
            catch { throw; }
            return serviceResponse;
        }

        public async Task<ServiceResponse<object>> DeleteAsync(int id)
        {
            var serviceResponse = new ServiceResponse<object>();
            try
            {
                var post = _postRepository.GetById(id);
                if (post != null)
                {
                    await _postRepository.DeleteAsync(post.id);
                    serviceResponse.ResponseType = EResponseType.Success;
                    serviceResponse.Message = "Delete post successfully!";
                }
                else
                {
                    serviceResponse.ResponseType = EResponseType.BadRequest;
                    serviceResponse.Message = "Post deletion failed. The post has been deleted.";
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new DbUpdateConcurrencyException(CJConstant.SOMETHING_WENT_WRONG);
            }
            catch { throw; }
            return serviceResponse;
        }

        public async Task<ServiceResponse<object>> ActiveAsync(int id)
        {
            var serviceResponse = new ServiceResponse<object>();
            try
            {
                var post = _postRepository.GetById(id); 
                if (post != null && post.is_actived == false)
                {
                    await _postRepository.ActiveAsync(post.id);
                    serviceResponse.ResponseType = EResponseType.Success;
                    serviceResponse.Message = "Post has been successfully approved.";
                }
                else
                {
                    serviceResponse.ResponseType = EResponseType.BadRequest;
                    serviceResponse.Message = "Post approval failed. The post has been deleted or has been approved.";
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new DbUpdateConcurrencyException(CJConstant.SOMETHING_WENT_WRONG);
            }
            catch { throw; }
            return serviceResponse;
        }

        public async Task<ServiceResponse<object>> UndoDeletedAsync(int id)
        {
            var serviceResponse = new ServiceResponse<object>();
            try
            {
                var post = _postRepository.GetPostById(id);
                if (post != null)
                {
                    if (post.is_deleted == true)
                    {
                        await _postRepository.UndoDeletedAsync(post);
                        serviceResponse.ResponseType = EResponseType.Success;
                        serviceResponse.Message = "Post has been undo delete successfully.";
                    }
                    else
                    {
                        serviceResponse.ResponseType = EResponseType.BadRequest;
                        serviceResponse.Message = "The post has not been deleted.";
                    }
                }
                else
                {
                    serviceResponse.ResponseType = EResponseType.NotFound;
                    serviceResponse.Message = "Post not found";
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new DbUpdateConcurrencyException(CJConstant.SOMETHING_WENT_WRONG);
            }
            catch { throw; }
            return serviceResponse;
        }

        public async Task<ServiceResponse<PagingReturnModel<PostDetailsDTO>>> GetAllAsync(FilterOptions? filterParameters)
        {
            var predicate = PredicateBuilder.New<PostDetailsDTO>();
            predicate = predicate.Or(p => p.title.Contains(filterParameters.search_term));
            predicate = predicate.Or(p => p.caption.Contains(filterParameters.search_term));
            predicate = predicate.Or(p => p.author.Contains(filterParameters.search_term));

            var serviceResponse = new ServiceResponse<PagingReturnModel<PostDetailsDTO>>();
            try
            {
                var posts = _mapper.ProjectTo<PostDetailsDTO>(_postRepository.GetPosts())
                        .Where(predicate)
                        .AsNoTracking();
                #region apply sorting and paging
                var sortedPosts = _filterHelper2.ApplySorting(posts, filterParameters?.order_by);
                var pagedPosts = await _filterHelper2.ApplyPaging(sortedPosts, filterParameters!.page, filterParameters.limit);
                #endregion
                serviceResponse.ResponseType = EResponseType.Success;
                serviceResponse.Data = pagedPosts;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new DbUpdateConcurrencyException(CJConstant.SOMETHING_WENT_WRONG);
            }
            catch { throw; }
            return serviceResponse;
        }

        public async Task<ServiceResponse<PagingReturnModel<PostValidatorDTO>>> FilterAllAsync(int user_id, FilterOptions? filter_parameters, DateDTO? date_filter, string? status_filter)
        {
            var role_id = _userRepository.GetRoleByUserId(user_id);
            var predicate = PredicateBuilder.New<PostValidatorDTO>();
            predicate = predicate.Or(p => p.title.Contains(filter_parameters.search_term));
            predicate = predicate.Or(p => p.caption.Contains(filter_parameters.search_term));
            predicate = predicate.Or(p => p.author.Contains(filter_parameters.search_term));

            var serviceResponse = new ServiceResponse<PagingReturnModel<PostValidatorDTO>>();
            try
            {
                var posts = _mapper.ProjectTo<PostValidatorDTO>(_postRepository.GetPosts())
                        .IgnoreQueryFilters().Where(p => (!p.is_deleted) || (p.is_deleted && (p.changed_by == role_id || p.changed_by == 0)))
                        .Where(predicate)
                        .AsNoTracking();
                #region filter status of post
                if (status_filter == CJConstant.IS_DELETED)
                    posts = _mapper.ProjectTo<PostValidatorDTO>(_postRepository.GetSoftDelete())
                        .Where(p => p.changed_by == role_id || p.changed_by == 0)
                        .Where(predicate)
                        .AsNoTracking();
                else if (status_filter == CJConstant.IS_ACTIVED)
                    posts = posts.Where(p => p.is_actived == true);
                else if (status_filter == CJConstant.NOT_YET_APPROVED)
                    posts = posts.Where(p => p.is_deleted == false && p.is_actived == false);
                #endregion
                #region filter create date of post
                if (date_filter?.start_date != null)
                    posts = posts.Where(p => p.created_at >= date_filter.start_date);
                if (date_filter?.end_date != null)
                    posts = posts.Where(p => p.created_at <= date_filter.end_date);
                #endregion
                #region apply sorting and paging
                var sortedPosts = _filterHelper.ApplySorting(posts, filter_parameters!.order_by!);
                var pagedPosts = await _filterHelper.ApplyPaging(sortedPosts, filter_parameters.page, filter_parameters.limit);
                #endregion
                serviceResponse.ResponseType = EResponseType.Success;
                serviceResponse.Data = pagedPosts;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new DbUpdateConcurrencyException(CJConstant.SOMETHING_WENT_WRONG);
            }
            catch { throw; }
            return serviceResponse;
        }
    }
}
