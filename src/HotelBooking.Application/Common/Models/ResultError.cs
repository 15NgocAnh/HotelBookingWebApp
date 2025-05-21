using System.Text.Json.Serialization;

namespace HotelBooking.Application.Common.Models;

public class ResultError
{
    [JsonPropertyName("field")]
    public string Field { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }

    public ResultError()
    {
    }

    public ResultError(string errorMessage)
    {
        Message = errorMessage;
    }

    public ResultError(string field, string errorMessage)
    {
        Field = field;
        Message = errorMessage;
    }
}