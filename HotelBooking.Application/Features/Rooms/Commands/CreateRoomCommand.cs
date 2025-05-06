using AutoMapper;
using FluentValidation;
using HotelBooking.Application.Common.Interfaces;
using HotelBooking.Application.Common.Models;
using HotelBooking.Domain.Entities;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.Features.Rooms.Commands;

public class CreateRoomCommand : ICommand<int>
{
    public string RoomNumber { get; set; }
    public string RoomType { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
}

public class CreateRoomCommandValidator : AbstractValidator<CreateRoomCommand>
{
    public CreateRoomCommandValidator()
    {
        RuleFor(x => x.RoomNumber)
            .NotEmpty().WithMessage("Room number is required")
            .MaximumLength(10).WithMessage("Room number must not exceed 10 characters");

        RuleFor(x => x.RoomType)
            .NotEmpty().WithMessage("Room type is required");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}

public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, BaseResponse<int>>
{
    private readonly IRoomRepository _roomRepository;
    private readonly IMapper _mapper;

    public CreateRoomCommandHandler(IRoomRepository roomRepository, IMapper mapper)
    {
        _roomRepository = roomRepository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<int>> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        var room = _mapper.Map<Room>(request);
        var createdRoom = await _roomRepository.AddAsync(room);
        
        return new BaseResponse<int>(createdRoom.Id, "Room created successfully");
    }
} 