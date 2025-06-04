using HotelBooking.Application.Common.Exceptions;
using HotelBooking.Domain.AggregateModels.InvoiceAggregate;
using HotelBooking.Domain.Common;
using HotelBooking.Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace HotelBooking.Application.CQRS.Invoice.Commands.CreateInvoice;

public class CreateInvoiceCommandHandler : IRequestHandler<CreateInvoiceCommand, Result<int>>
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IRoomTypeRepository _roomTypeRepository;
    private readonly IExtraItemRepository _extraItemRepository;
    private readonly IExtraCategoryRepository _extraCategoryRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateInvoiceCommandHandler> _logger;

    public CreateInvoiceCommandHandler(
        IInvoiceRepository invoiceRepository,
        IBookingRepository bookingRepository,
        IRoomRepository roomRepository,
        IRoomTypeRepository roomTypeRepository,
        IExtraItemRepository extraItemRepository,
        IExtraCategoryRepository extraCategoryRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork,
        ILogger<CreateInvoiceCommandHandler> logger)
    {
        _invoiceRepository = invoiceRepository;
        _bookingRepository = bookingRepository;
        _roomRepository = roomRepository;
        _roomTypeRepository = roomTypeRepository;
        _extraItemRepository = extraItemRepository;
        _extraCategoryRepository = extraCategoryRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<int>> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Validate booking exists and is in a valid state
            var booking = await _bookingRepository.GetByIdAsync(request.BookingId);
            if (booking == null)
            {
                throw new NotFoundException($"Booking with ID {request.BookingId} not found");
            }

            if (!booking.CanGenerateInvoice())
            {
                throw new DomainException($"Cannot create invoice for booking in {booking.Status} status");
            }

            // Check if an invoice already exists for this booking
            var existingInvoice = await _invoiceRepository.GetByBookingIdAsync(request.BookingId);
            if (existingInvoice != null)
            {
                _logger.LogInformation("Invoice {InvoiceId} already exists for booking {BookingId}, returning existing invoice",
                    existingInvoice.Id, request.BookingId);
                return Result<int>.Success(existingInvoice.Id);
            }

            // Create the invoice
            var invoice = new Domain.AggregateModels.InvoiceAggregate.Invoice(
                request.BookingId,
                booking.CheckOutTime?.AddDays(2) ?? DateTime.Now.AddDays(2),
                request.Notes
            );

            var invoiceNumber = await GenerateInvoiceNumberAsync();
            invoice.SetInvoiceNumber(invoiceNumber);

            var room = await _roomRepository.GetByIdAsync(booking.RoomId);
            var roomType = await _roomTypeRepository.GetByIdAsync(room.RoomTypeId);

            invoice.CalculateTotalAmount(roomType.Price);

            // Create invoice items from DTOs
            var invoiceItems = new List<InvoiceItem>
            {
                new InvoiceItem("Room", 1, roomType.Price, room.Name)
            };

            // Early Check-in
            if (booking.CheckInTime.HasValue && booking.CheckInTime < booking.CheckInDate.AddHours(14)) // giả sử giờ quy định là 14h
            {
                var hoursEarly = (booking.CheckInDate.AddHours(14) - booking.CheckInTime.Value).TotalHours;
                var earlyCheckInFee = roomType.Price * 0.5m * (decimal)(hoursEarly / 24);

                invoiceItems.Add(new InvoiceItem(
                    $"Early Check-in Fee ({Math.Round(hoursEarly, 1)} hours)",
                    1,
                    Math.Round(earlyCheckInFee),
                    "Early Check-in"
                ));
            }

            // Late Check-out
            if (booking.CheckOutTime.HasValue && booking.CheckOutTime > booking.CheckOutDate.AddHours(12)) // giả sử giờ quy định là 12h
            {
                var hoursLate = (booking.CheckOutTime.Value - booking.CheckOutDate.AddHours(12)).TotalHours;
                var lateCheckOutFee = roomType.Price * 0.5m * (decimal)(hoursLate / 24);

                invoiceItems.Add(new InvoiceItem(
                    $"Late Check-out Fee ({Math.Round(hoursLate, 1)} hours)",
                    1,
                    Math.Round(lateCheckOutFee),
                    "Late Check-out"
                ));
            }

            foreach (var item in booking.ExtraUsages)
            {
                var extraItem = await _extraItemRepository.GetByIdAsync(item.ExtraItemId);
                var extraCategory = await _extraCategoryRepository.GetByIdAsync(extraItem.ExtraCategoryId);
                invoiceItems.Add(new InvoiceItem(item.ExtraItemName, item.Quantity, extraItem.Price, extraCategory.Name));
            }

            invoice.AddRangeItem(invoiceItems);

            // Add invoice to repository
            await _invoiceRepository.AddAsync(invoice);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Created invoice {InvoiceId} ({InvoiceNumber}) for booking {BookingId}",
                invoice.Id, invoice.InvoiceNumber, request.BookingId);

            return Result<int>.Success(invoice.Id);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Domain error while creating invoice for booking {BookingId}", 
                request.BookingId);
            return Result<int>.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating invoice for booking {BookingId}", 
                request.BookingId);
            return Result<int>.Failure("An error occurred while creating the invoice");
        }
    }

    private async Task<string> GenerateInvoiceNumberAsync()
    {
        var today = DateTime.UtcNow.Date;
        var countToday = await _invoiceRepository.CountAsync(i => i.CreatedAt.Date == DateTime.Today.Date);

        return $"INV-{today:yyyyMMdd}-{countToday + 1:0000}";
    }
} 