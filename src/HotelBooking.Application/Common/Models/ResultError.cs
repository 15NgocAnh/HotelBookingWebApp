namespace HotelBooking.Application.Common.Models;

public class ResultError
{
    public string Identifier { get; init; } = "GeneralErrors";
    public string ErrorMessage { get; init; }

    public ResultError(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }

    public ResultError(string identifier, string errorMessage)
    {
        Identifier = identifier;
        ErrorMessage = errorMessage;
    }
}