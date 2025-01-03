using TickerAlert.Application.Interfaces.Cedears.Dtos;

namespace TickerAlert.Application.Interfaces.Cedears;

public interface ICedearInformationCacheService
{
    Task<CedearInformationDto?> GetCedearInformation(Guid financialAssetId);
    Task UpdateCedearInformation(Guid financialAssetId, CedearInformationDto cedearInformation);
}
