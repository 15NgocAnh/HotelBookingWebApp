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
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateInvoiceCommandHandler> _logger;

    public CreateInvoiceCommandHandler(
        IInvoiceRepository invoiceRepository,
        IBookingRepository bookingRepository,
        IRoomRepository roomRepository,
        IRoomTypeRepository roomTypeRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork,
        ILogger<CreateInvoiceCommandHandler> logger)
    {
        _invoiceRepository = invoiceRepository;
        _bookingRepository = bookingRepository;
        _roomRepository = roomRepository;
        _roomTypeRepository = roomTypeRepository;
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
                throw new DomainException($"An invoice already exists for booking {request.BookingId}");
            }

            // Create the invoice
            var invoice = new Domain.AggregateModels.InvoiceAggregate.Invoice(
                request.BookingId,
                booking.CheckOutTime?.AddDays(2) ?? DateTime.Now.AddDays(2),
                request.PaymentMethod,
                request.Notes
            );

            var invoiceNumber = await GenerateInvoiceNumberAsync();
            invoice.SetInvoiceNumber(invoiceNumber);

            // Create invoice items from DTOs
            //var invoiceItems = booking.ExtraUsages.Select(item => new InvoiceItem(
            //    item.Description,
            //    item.Quantity,
            //    item.UnitPrice,
            //    item.Type
            //)).ToList();

            //invoice.AddRangeItem(invoiceItems);

            var room = await _roomRepository.GetByIdAsync(booking.RoomId);
            if (room != null)
            {
                var roomType = await _roomTypeRepository.GetByIdAsync(room.RoomTypeId);
                if (roomType != null)
                {
                    invoice.CalculateTotalAmount(roomType.Price);
                }
            }

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