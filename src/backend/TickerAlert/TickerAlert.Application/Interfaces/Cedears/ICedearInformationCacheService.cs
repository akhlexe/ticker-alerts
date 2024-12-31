using TickerAlert.Application.Interfaces.Cedears.Dtos;

namespace TickerAlert.Application.Interfaces.Cedears;

internal interface ICedearInformationCacheService
{
    Task<CedearInformationDto?> GetCedearInformation(Guid financialAssetId);
    Task UpdateCedearInformation(Guid financialAssetId, CedearInformationDto cedearInformation);
}
