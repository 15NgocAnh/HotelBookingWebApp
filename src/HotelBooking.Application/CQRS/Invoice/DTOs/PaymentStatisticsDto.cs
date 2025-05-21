namespace HotelBooking.Application.CQRS.Invoice.DTOs
{
    public class PaymentStatisticsDto
    {
        public decimal TotalPayments { get; set; }
        public decimal AveragePaymentAmount { get; set; }
        public double AverageTimeToPayment { get; set; } // in days
        public int TotalTransactions { get; set; }
        public List<PaymentMethodStatisticsDto> PaymentMethodStatistics { get; set; } = new();
    }

    public class PaymentMethodStatisticsDto
    {
        public string PaymentMethod { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public int TransactionCount { get; set; }
        public double AverageTimeToPayment { get; set; } // in days
    }
} 