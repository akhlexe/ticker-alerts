using TickerAlert.Application.Common.Cache;
using TickerAlert.Application.Interfaces.Cedears;
using TickerAlert.Application.Interfaces.Cedears.Dtos;

namespace TickerAlert.Application.Services.Cedears;

public class CedearInformationCacheService(ICacheService cacheService) : ICedearInformationCacheService
{
    private const string Namespace = "Cedears";
    private const int ExpirationInDays = 1;

    public Task<CedearInformationDto?> GetCedearInformation(Guid financialAssetId)
    {
        return cacheService.GetAsync<CedearInformationDto?>(Namespace, financialAssetId.ToString());
    }

    public async Task UpdateCedearInformation(Guid financialAssetId, CedearInformationDto cedearInformation)
    {
        await cacheService.SetAsync(Namespace, financialAssetId.ToString(), cedearInformation, TimeSpan.FromDays(ExpirationInDays));
    }
}
