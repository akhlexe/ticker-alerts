using MediatR;
using TickerAlert.Application.Common.Responses;

namespace TickerAlert.Application.UseCases.Alerts.CreateAlert;

public record class CreateAlertCommand(string Ticker, decimal TargetPrice) : IRequest<Result>;

public class CreateAlertCommandHandler : IRequestHandler<CreateAlertCommand, Result>
{
    public Task<Result> Handle(CreateAlertCommand request, CancellationToken cancellationToken)
    {
        Console.WriteLine(request);
        Console.WriteLine(request.Ticker);

        return Task.FromResult(Result.SuccessResult());
    }
}
