namespace HotelBooking.Application.CQRS.Invoice.DTOs
{
    public class RevenueStatisticsDto
    {
        public decimal TotalRevenue { get; set; }
        public List<TimePeriodRevenueDto> RevenueByTimePeriod { get; set; } = new();
        public List<PaymentMethodDistributionDto> RevenueByPaymentMethod { get; set; } = new();
    }

    public class TimePeriodRevenueDto
    {
        public string Period { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public int InvoiceCount { get; set; }
    }

    public class PaymentMethodDistributionDto
    {
        public string PaymentMethod { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public int TransactionCount { get; set; }
    }
} 