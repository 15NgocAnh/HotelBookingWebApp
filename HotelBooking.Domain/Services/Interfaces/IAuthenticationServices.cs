using HotelBooking.Domain.DTOs.Authentication;
using HotelBooking.Domain.DTOs.User;
using HotelBooking.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Domain.Services.Interfaces
{
    public interface IAuthenticationServices
    {
        /// <summary>
        /// Verify Login information of user.
        /// </summary>
        /// <param name="userdata">User login data include: Username, Password.</param>

        Task<ServiceResponse<CredentialDTO>> LoginAsync(UserLoginDTO userdata);
        /// <summary>
        /// Send an email with activation link to user email box.
        /// </summary>
        /// <param name="userid">The id collect from Cookie of User request.</param>

        Task verifyEmailAsync(string userid);
        /// <summary>
        /// Verify token attach in mail box. If it's valid then change the user email confirm to True
        /// </summary>
        /// <param name="Token">Token attach in the email box.</param>

        Task<ServiceResponse<object>> activeEmailAsync(string Token);

        /// <summary>
        /// When ever token of user expire. This method verify refreshtoken to reissue new short term token.
        /// </summary>
        /// <param name="reftoken">Long term token store in User storage.</param>

        Task<ServiceResponse<TokenDTO>> refreshTokenAsync(string reftoken);

        /// <summary>
        /// Send an email with forgot link to user email box
        /// </summary>
        /// <param name="useremail">Thông tin đăng nhập của người dùng</param>
        Task<ServiceResponse<object>> sendForgotEmailVerify(string useremail);
        /// <summary>
        /// Invalidate a refresh token store in DB
        /// </summary>
        /// <param name="reftoken">Long term token store in User storage.</param>
        Task<ServiceResponse<object>> logout(string reftoken);
    }
}
