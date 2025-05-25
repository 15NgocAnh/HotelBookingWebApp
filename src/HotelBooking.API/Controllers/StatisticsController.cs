using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.Statistic.Dtos;
using HotelBooking.Application.CQRS.Statistic.Queries.GetBookingStatistics;
using HotelBooking.Application.CQRS.Statistic.Queries.GetRevenueStatistics;
using HotelBooking.Application.CQRS.Statistic.Queries.GetRoomStatistics;
using HotelBooking.Application.Services.Report;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using QuestPDF.Fluent;

namespace HotelBooking.API.Controllers
{
    [Authorize]
    [Authorize(Roles = "SuperAdmin,HotelManager")]
    public class StatisticsController : BaseController
    {
        public StatisticsController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("booking")]
        public async Task<IActionResult> GetBookingStatistics(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            var query = new GetBookingStatisticsQuery
            {
                StartDate = startDate,
                EndDate = endDate
            };
            var result = await _mediator.Send(query);
            return HandleResult(result);
        }

        [HttpGet("revenue")]
        public async Task<IActionResult> GetRevenueStatistics(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            var query = new GetRevenueStatisticsQuery
            {
                StartDate = startDate,
                EndDate = endDate
            };
            var result = await _mediator.Send(query);
            return HandleResult(result);
        }

        [HttpGet("room")]
        public async Task<IActionResult> GetRoomStatistics()
        {
            var query = new GetRoomStatisticsQuery();
            var result = await _mediator.Send(query);
            return HandleResult(result);
        }

        [HttpPost("export")]
        public async Task<IActionResult> ExportReport([FromBody] ExportReportRequest request)
        {
            var bookingQuery = new GetBookingStatisticsQuery
            {
                StartDate = request.StartDate,
                EndDate = request.EndDate
            };
            var bookingResult = await _mediator.Send(bookingQuery);

            var revenueQuery = new GetRevenueStatisticsQuery
            {
                StartDate = request.StartDate,
                EndDate = request.EndDate
            };
            var revenueResult = await _mediator.Send(revenueQuery);

            var roomQuery = new GetRoomStatisticsQuery();
            var roomResult = await _mediator.Send(roomQuery);

            if (request.Format?.ToLower() == "pdf")
            {
                var doc = new ReportPdfDocument(bookingResult?.Data, revenueResult?.Data, roomResult?.Data);
                var pdfBytes = doc.GeneratePdf();
                var result = new Result<byte[]>() { IsSuccess = true, Data = pdfBytes };
                return HandleResult(result);
            }
            else
            {
                var doc = new ReportExcelDocument(bookingResult?.Data, revenueResult?.Data, roomResult?.Data);
                var excelBytes = doc.Generate();
                var result = new Result<byte[]>() { IsSuccess = true, Data = excelBytes };
                return HandleResult(result);
            }
        }
    }
}