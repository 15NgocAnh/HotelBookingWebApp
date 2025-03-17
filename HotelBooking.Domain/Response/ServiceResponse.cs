using HotelBooking.Domain.DTOs.Common;
using static HotelBooking.Domain.Response.EServiceResponseTypes;

namespace HotelBooking.Domain.Response
{
    public class ServiceResponse<T>
    {
        public T Data { get; set; }
        public EResponseType ResponseType { get; set; }
        public string Message { get; set; } = null;
        public CommonResponseDTO getMessage()
        {
            return new CommonResponseDTO()
            {
                status_code = (int)ResponseType,
                message = Message
            };
        }
        public CommonResponseDataDTO<T> getData()
        {
            return new CommonResponseDataDTO<T>()
            {
                status_code = (int)ResponseType,
                data = Data
            };
        }
    }
}