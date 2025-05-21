namespace HotelBooking.Application.CQRS.ExtraItem.DTOs
{
    public class ExtraItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ExtraCategoryId { get; set; }
        public string? ExtraCategoryName { get; set; }
        public decimal Price { get; set; }
    }
} 