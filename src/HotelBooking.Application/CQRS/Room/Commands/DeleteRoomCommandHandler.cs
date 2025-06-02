using HotelBooking.Application.Common.Models;
using HotelBooking.Domain.Common;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.Room.Commands
{
    public class DeleteRoomCommandHandler : IRequestHandler<DeleteRoomCommand, Result>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteRoomCommandHandler(
            IRoomRepository roomRepository,
            IUnitOfWork unitOfWork)
        {
            _roomRepository = roomRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var room = await _roomRepository.GetByIdAsync(request.Id);
                if (room == null)
                {
                    return Result.Failure("Room not found");
                }

                // Check if room has active bookings
                if (await _roomRepository.HasActiveBookingsAsync(request.Id))
                {
                    return Result.Failure("Cannot delete room with active bookings");
                }

                await _roomRepository.DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Failed to delete room: {ex.Message}");
            }
        }
    }
} 