using HotelBooking.Application.Common.Models;
using MediatR;

namespace HotelBooking.Application.Common.Interfaces;

public interface ICommand<TResponse> : IRequest<BaseResponse<TResponse>>
{
} 