using HotelBooking.Application.CQRS.Booking.DTOs;
using HotelBooking.Application.CQRS.Invoice.DTOs;
using HotelBooking.Domain.AggregateModels.InvoiceAggregate;
using HotelBooking.Domain.Interfaces.Repositories;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Unit = QuestPDF.Infrastructure.Unit;

namespace HotelBooking.Application.CQRS.Invoice.Commands.GenerateInvoicePdf;

public class GenerateInvoicePdfCommandHandler : IRequestHandler<GenerateInvoicePdfCommand, Result<byte[]>>
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GenerateInvoicePdfCommandHandler(
        IInvoiceRepository invoiceRepository,
        IBookingRepository bookingRepository,
        IUserRepository userRepository, 
        IMapper mapper)
    {
        _invoiceRepository = invoiceRepository;
        _bookingRepository = bookingRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<Result<byte[]>> Handle(GenerateInvoicePdfCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var invoice = await _invoiceRepository.GetByIdAsync(request.InvoiceId);
            if (invoice == null)
                return Result<byte[]>.Failure("Invoice not found");

            var invoiceDto = _mapper.Map<InvoiceDto>(invoice);
            var user = await _userRepository.GetByIdAsync(int.Parse(invoiceDto.CreatedBy));
            invoiceDto.CreatedBy = $"{user?.FirstName} {user?.LastName} (NV{user?.Id:0000})";

            var booking = await _bookingRepository.GetByIdAsync(invoiceDto.BookingId);
            invoiceDto.Guests = _mapper.Map<List<GuestDto>>(booking.Guests);

            var document = new InvoicePdfDocument(invoiceDto);
            var pdfBytes = document.GeneratePdf();
            return Result<byte[]>.Success(pdfBytes);
        }
        catch (Exception ex)
        {

            throw;
        }
    }
}

public class InvoicePdfDocument : IDocument
{
    private readonly InvoiceDto _invoice;

    public InvoicePdfDocument(InvoiceDto invoice)
    {
        _invoice = invoice;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Size(PageSizes.A4);
            page.Margin(2, Unit.Centimetre);
            page.DefaultTextStyle(x => x.FontSize(10).FontColor(Colors.Black));

            page.Header().Element(ComposeHeader);
            page.Content().Element(ComposeContent);
            page.Footer().Element(ComposeFooter);
        });
    }

    private void ComposeHeader(IContainer container)
    {
        container.Column(column =>
        {
            column.Item().AlignCenter().Text("HOTEL BOOKING").FontSize(20).Bold().FontColor(Colors.Blue.Darken2);
            column.Item().AlignCenter().Text("123 Đường ABC, Quận XYZ, TP.HCM").FontSize(10);
            column.Item().AlignCenter().Text($"Điện thoại: (028) 1234 5678 | Email: info@hotelbooking.com | MST: 1234567890").FontSize(10);

            var logoPath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Images", "logo.png");
            column.Item().AlignCenter().Height(50).Image(logoPath);

            column.Item().BorderBottom(1).PaddingBottom(5);
        });
    }

    private void ComposeContent(IContainer container)
    {
        container.Column(column =>
        {
            // Invoice Information
            column.Item().PaddingTop(10).AlignCenter()
                .Text($"HÓA ĐƠN #{_invoice.InvoiceNumber}").FontSize(16).Bold();
            column.Item().AlignCenter().Text($"Ngày tạo: {_invoice.CreatedAt:dd/MM/yyyy}").FontSize(10);
            column.Item().AlignCenter().Text($"Nhân viên lập: {_invoice.CreatedBy}").FontSize(10);
            column.Item().AlignCenter().Text($"Trạng thái: {GetStatusText(_invoice.Status)}").FontSize(10);
            column.Item().BorderBottom(1).PaddingBottom(10);

            // Customer Information
            column.Item().PaddingTop(10).Text("THÔNG TIN KHÁCH HÀNG").FontSize(12).Bold().Underline();
            column.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(2);
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                table.Header(header =>
                {
                    header.Cell().Element(cell => cell.BorderBottom(1).PaddingBottom(2).Text("Họ và tên").Bold());
                    header.Cell().Element(cell => cell.BorderBottom(1).PaddingBottom(2).Text("Số điện thoại").Bold());
                    header.Cell().Element(cell => cell.BorderBottom(1).PaddingBottom(2).Text("CMND/Passport").Bold());
                });

                foreach (var guest in _invoice.Guests)
                {
                    table.Cell().Text($"{guest.FirstName} {guest.LastName}");
                    table.Cell().Text(guest.PhoneNumber);
                    table.Cell().Text(guest.CitizenIdNumber ?? guest.PassportNumber);
                }
            });

            // Invoice Details
            column.Item().PaddingTop(10).Text("CHI TIẾT HÓA ĐƠN").FontSize(12).Bold().Underline();
            column.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(3);
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                table.Header(header =>
                {
                    header.Cell().Element(cell => cell.BorderBottom(1).PaddingBottom(2).Text("Mô tả").Bold());
                    header.Cell().Element(cell => cell.BorderBottom(1).PaddingBottom(2).AlignRight().Text("Số lượng").Bold());
                    header.Cell().Element(cell => cell.BorderBottom(1).PaddingBottom(2).AlignRight().Text("Thành tiền").Bold());
                });

                table.Cell().Text("Phí phòng");
                table.Cell().AlignRight().Text("1");
                table.Cell().AlignRight().Text(_invoice.Items.First(i => i.Description == "Room").UnitPrice.ToString("N0") + " VNĐ");

                foreach (var service in _invoice.Items.Where(i => i.Description != "Room"))
                {
                    table.Cell().Text(service.Description);
                    table.Cell().AlignRight().Text(service.Quantity.ToString());
                    table.Cell().AlignRight().Text((service.UnitPrice * service.Quantity).ToString("N0") + " VNĐ");
                }

                table.Cell().Text("Tổng cộng").Bold();
                table.Cell().Text("").Bold();
                table.Cell().AlignRight().Text(_invoice.TotalAmount.ToString("N0") + " VNĐ").Bold();
            });

            // Payment Information
            column.Item().PaddingTop(10).Text("THÔNG TIN THANH TOÁN").FontSize(12).Bold().Underline();
            column.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                table.Cell().Text("Số tiền đã thanh toán").Bold();
                table.Cell().Text(_invoice.PaidAmount.ToString("N0") + " VNĐ");

                table.Cell().Text("Số tiền còn lại").Bold();
                table.Cell().Text((_invoice.TotalAmount - _invoice.PaidAmount).ToString("N0") + " VNĐ");
            });
        });
    }

    private void ComposeFooter(IContainer container)
    {
        container.AlignCenter().Column(column =>
        {
            column.Item().Text("Cảm ơn quý khách đã sử dụng dịch vụ của chúng tôi!").FontSize(10);
            column.Item().Text($"Xuất hóa đơn lúc: {DateTime.Now:dd/MM/yyyy HH:mm:ss}").FontSize(8);
            column.Item().Text("Cửa hàng xuất hóa đơn sử dụng trong vòng 30 ngày kể từ ngày xuất hóa đơn").FontSize(8);
        });
    }

    private string GetStatusText(InvoiceStatus status)
    {
        return status switch
        {
            InvoiceStatus.Pending => "Chờ thanh toán",
            InvoiceStatus.Paid => "Đã thanh toán",
            InvoiceStatus.Cancelled => "Đã hủy",
            _ => status.ToString()
        };
    }
}