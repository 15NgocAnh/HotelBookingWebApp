namespace HotelBooking.Domain.Exceptions
{
    public class InvoiceDomainException : DomainException
    {
        public InvoiceDomainException()
        { }

        public InvoiceDomainException(string message)
            : base(message)
        { }

        public InvoiceDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
} 