namespace TickerAlert.Application.Interfaces.NotificationService.Dtos;

public sealed record AssetPriceUpdateDto(Guid FinancialAssetId, decimal NewPrice);
