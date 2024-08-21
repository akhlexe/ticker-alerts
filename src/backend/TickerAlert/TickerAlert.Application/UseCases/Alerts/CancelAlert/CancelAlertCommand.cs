using MediatR;
using TickerAlert.Application.Common.Responses;
using TickerAlert.Application.Interfaces.Alerts;

namespace TickerAlert.Application.UseCases.Alerts.CancelAlert;

public sealed record CancelAlertCommand(Guid alertId) : IRequest<Result>;

public sealed class CancelAlertCommandHandler(IAlertService alertService) : IRequestHandler<CancelAlertCommand, Result>
{
    private readonly IAlertService _alertService = alertService;

    public async Task<Result> Handle(CancelAlertCommand request, CancellationToken cancellationToken)
    {
        await _alertService.CancelAlert(request.alertId);
        return Result.SuccessResult();
    }
}
