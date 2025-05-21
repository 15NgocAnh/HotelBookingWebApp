namespace HotelBooking.Application.CQRS.Invoice.DTOs
{
    public class PaymentHistoryDto
    {
        public int InvoiceId { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public decimal InvoiceAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        public List<PaymentTransactionDto> Transactions { get; set; } = new();
    }

    public class PaymentTransactionDto
    {
        public int Id { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string? TransactionId { get; set; }
        public string? Notes { get; set; }
        public string Status { get; set; } = string.Empty;
    }
} 