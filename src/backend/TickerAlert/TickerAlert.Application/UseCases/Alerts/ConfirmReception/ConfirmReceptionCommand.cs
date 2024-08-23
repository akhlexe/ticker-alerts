using MediatR;
using TickerAlert.Application.Common.Responses;
using TickerAlert.Application.Interfaces.Alerts;

namespace TickerAlert.Application.UseCases.Alerts.ConfirmReception;

public sealed record ConfirmReceptionCommand(Guid Id) : IRequest<Result>;

public sealed class ConfirmReceptionCommandHandler(IAlertService alertService) : IRequestHandler<ConfirmReceptionCommand, Result>
{
    private readonly IAlertService _alertService = alertService;

    public async Task<Result> Handle(ConfirmReceptionCommand request, CancellationToken cancellationToken)
    {
        await _alertService.ConfirmReception(request.Id);
        return Result.SuccessResult();
    }
}