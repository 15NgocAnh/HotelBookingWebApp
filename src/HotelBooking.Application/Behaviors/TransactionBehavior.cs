using HotelBooking.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HotelBooking.Application.Behaviors;

public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IUnitOfWorkWithTransaction _unitOfWork;
    private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;

    public TransactionBehavior(
        IUnitOfWorkWithTransaction unitOfWork,
        ILogger<TransactionBehavior<TRequest, TResponse>> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var isCommand = requestName.EndsWith("Command");
        
        // Only apply transaction to Commands, not Queries
        if (!isCommand)
        {
            return await next();
        }
        
        _logger.LogInformation("Beginning transaction for {RequestName}", requestName);
        
        var response = default(TResponse);

        await _unitOfWork.ExecuteWithTransactionAsync(async () =>
        {
            response = await next();
            _logger.LogInformation("Committed transaction for {RequestName}", requestName);
        }, cancellationToken);

        return response;
    }
} 