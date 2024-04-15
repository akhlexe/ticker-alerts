using MediatR;
using TickerAlert.Application.Common.Responses;
using TickerAlert.Application.Interfaces.Alerts;

namespace TickerAlert.Application.UseCases.Alerts.CreateAlert;

public record class CreateAlertCommand(int FinancialAssetId, decimal TargetPrice) : IRequest<Result>;

public class CreateAlertCommandHandler : IRequestHandler<CreateAlertCommand, Result>
{
    private readonly IAlertService _alertService;

    public CreateAlertCommandHandler(IAlertService alertService)
    {
        _alertService = alertService;
    }

    public async Task<Result> Handle(CreateAlertCommand request, CancellationToken cancellationToken)
    {
        await _alertService.CreateAlert(request.FinancialAssetId, request.TargetPrice);

        return Result.SuccessResult();
    }
}
