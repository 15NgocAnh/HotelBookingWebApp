namespace HotelBooking.Application.Common.Models;

public class BaseResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
    public List<string> Errors { get; set; }

    public BaseResponse()
    {
        Success = true;
        Errors = new List<string>();
    }

    public BaseResponse(T data, string message = null)
    {
        Success = true;
        Message = message;
        Data = data;
        Errors = new List<string>();
    }

    public BaseResponse(string message)
    {
        Success = false;
        Message = message;
        Errors = new List<string>();
    }
} 