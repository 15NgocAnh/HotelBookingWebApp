namespace HotelBooking.Application.Common.Models;

public class Result
{
    public bool IsSuccess { get; set; }
    public ResultError[] Messages { get; set; } = [];

    public static Result Failure(string message)
        => new Result { IsSuccess = false, Messages = [new ResultError(message)] }; 
    
    public static Result Failure(string message, Exception? ex = null)
        => new Result { IsSuccess = false, Messages = [new ResultError(message, ex.ToString())] };

    public static Result Success() => new Result { IsSuccess = true };
}

public class Result<T>
{
    public T? Data { get; set; }
    public bool IsSuccess { get; set; }
    public ResultError[] Messages { get; set; } = [];

    public static Result<T> Success(T data)
        => new Result<T> { IsSuccess = true, Data = data };

    public static Result<T> Failure(string message)
        => new Result<T> { IsSuccess = false, Messages = [new ResultError(message)] };

    public new static Result<T> Failure(string message, Exception? ex = null)
        => new Result<T>
        {
            IsSuccess = false,
            Messages = [new ResultError(message, ex?.ToString())]
        };
}
