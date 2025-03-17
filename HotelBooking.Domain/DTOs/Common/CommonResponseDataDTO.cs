
namespace HotelBooking.Domain.DTOs.Common
{
    public class CommonResponseDataDTO<T>
    {
        public T data { get; set; } 
        public int status_code { get; set; }
    }
}
