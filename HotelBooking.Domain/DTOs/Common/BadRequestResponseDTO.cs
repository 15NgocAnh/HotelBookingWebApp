
namespace HotelBooking.Domain.DTOs.Common
{
    public class BadRequestResponseDTO
    {
        public string status { get; set; }
        public ICollection<ErrorsFieldsRequest> errors { get; set; }
    }
    public class ErrorsFieldsRequest
    {
        public string field { get; set; }
        public string message { get; set; }

    }
}
