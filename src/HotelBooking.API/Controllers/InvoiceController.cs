using HotelBooking.Application.CQRS.Invoice.Commands.AddPayment;
using HotelBooking.Application.CQRS.Invoice.Commands.AddRoomDamage;
using HotelBooking.Application.CQRS.Invoice.Commands.CreateInvoice;
using HotelBooking.Application.CQRS.Invoice.Commands.DeleteInvoice;
using HotelBooking.Application.CQRS.Invoice.Commands.GenerateInvoicePdf;
using HotelBooking.Application.CQRS.Invoice.Commands.UpdateInvoiceStatus;
using HotelBooking.Application.CQRS.Invoice.Queries;

namespace HotelBooking.API.Controllers
{
    [Authorize]
    public class InvoiceController : BaseController
    {
        public InvoiceController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        public async Task<IActionResult> CreateInvoice([FromBody] CreateInvoiceCommand command)
        {
            var result = await _mediator.Send(command);
            return HandleResult(result);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateInvoiceStatus(int id, [FromBody] UpdateInvoiceStatusCommand command)
        {
            if (id != command.Id)
                return BadRequest("ID mismatch");

            var result = await _mediator.Send(command);
            return HandleResult(result);
        }

        [HttpPut("{id}/payment")]
        public async Task<IActionResult> AddPayment(int id, [FromBody] AddPaymentCommand command)
        {
            if (id != command.InvoiceId)
                return BadRequest("ID mismatch");

            var result = await _mediator.Send(command);
            return HandleResult(result);
        }

        [HttpPut("{id}/damage")]
        public async Task<IActionResult> AddRoomDamage(int id, [FromBody] AddRoomDamageCommand command)
        {
            if (id != command.InvoiceId)
                return BadRequest("ID mismatch");

            var result = await _mediator.Send(command);
            return HandleResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInvoiceById(int id)
        {
            var query = new GetInvoiceByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            return HandleResult(result);
        }

        [HttpGet("booking/{bookingId}")]
        public async Task<IActionResult> GetInvoiceByBookingId(int bookingId)
        {
            var query = new GetInvoiceByBookingIdQuery { BookingId = bookingId };
            var result = await _mediator.Send(query);
            return HandleResult(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllInvoices()
        {
            var query = new GetAllInvoicesQuery();
            var result = await _mediator.Send(query);
            return HandleResult(result);
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingInvoices([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            var query = new GetPendingInvoicesQuery { FromDate = fromDate, ToDate = toDate };
            var result = await _mediator.Send(query);
            return HandleResult(result);
        }

        [HttpGet("overdue")]
        public async Task<IActionResult> GetOverdueInvoices([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            var query = new GetOverdueInvoicesQuery { FromDate = fromDate, ToDate = toDate };
            var result = await _mediator.Send(query);
            return HandleResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(int id)
        {
            var command = new DeleteInvoiceCommand(id);
            var result = await _mediator.Send(command);
            return HandleResult(result);
        }

        [HttpGet("{id}/pdf")]
        public async Task<IActionResult> GeneratePdf(int id)
        {
            var command = new GenerateInvoicePdfCommand { InvoiceId = id };
            var result = await _mediator.Send(command);
            return HandleResult(result);
        }
    }
}
