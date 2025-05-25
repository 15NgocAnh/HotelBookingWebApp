namespace HotelBooking.Application.CQRS.Statistic.Dtos
{
    public class ExportReportRequest
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Format { get; set; } = "excel";
    }
}
