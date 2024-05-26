using MediatR;
using TickerAlert.Application.Common.Responses;
using TickerAlert.Application.Interfaces.Alerts;

namespace TickerAlert.Application.UseCases.Alerts.CreateAlert;

public record class CreateAlertCommand(Guid FinancialAssetId, decimal TargetPrice) : IRequest<Result<Guid>>;

public class CreateAlertCommandHandler : IRequestHandler<CreateAlertCommand, Result<Guid>>
{
    private readonly IAlertService _alertService;

    public CreateAlertCommandHandler(IAlertService alertService)
    {
        _alertService = alertService;
    }

    public async Task<Result<Guid>> Handle(CreateAlertCommand request, CancellationToken cancellationToken)
    {
        var alertId = await _alertService.CreateAlert(request.FinancialAssetId, request.TargetPrice);

        return Result<Guid>.SuccessResult(alertId);
    }
}
