using HotelBooking.Application.Common.Models;
using MediatR;

namespace HotelBooking.Application.Common.Interfaces;

public interface IQuery<TResponse> : IRequest<BaseResponse<TResponse>>
{
} 