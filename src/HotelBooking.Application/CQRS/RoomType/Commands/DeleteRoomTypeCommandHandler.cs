using HotelBooking.Application.Common.Models;
using HotelBooking.Domain.Common;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.RoomType.Commands
{
    public class DeleteRoomTypeCommandHandler : IRequestHandler<DeleteRoomTypeCommand, Result>
    {
        private readonly IRoomTypeRepository _roomTypeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteRoomTypeCommandHandler(
            IRoomTypeRepository roomTypeRepository,
            IUnitOfWork unitOfWork)
        {
            _roomTypeRepository = roomTypeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteRoomTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var roomType = await _roomTypeRepository.GetByIdAsync(request.Id);
                if (roomType == null)
                {
                    return Result.Failure("Room type not found");
                }

                // Check if room type has associated rooms
                if (await _roomTypeRepository.HasRoomsAsync(request.Id))
                {
                    return Result.Failure("Cannot delete room type with associated rooms");
                }

                await _roomTypeRepository.DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Failed to delete room type: {ex.Message}");
            }
        }
    }
} 