using Microsoft.EntityFrameworkCore;
using TickerAlert.Application.Common.Persistence;
using TickerAlert.Application.Interfaces.Cedears;
using TickerAlert.Application.Interfaces.Cedears.Dtos;
using TickerAlert.Domain.Entities;

namespace TickerAlert.Application.Services.Cedears;

public sealed class CedearInformationService(
    ICedearInformationCacheService cacheService, 
    IApplicationDbContext context)
{
    public async Task<CedearInformationDto> GetCedearInformationAsync(Guid financialAssetId)
    {
        var cedearInformation = await cacheService.GetCedearInformation(financialAssetId);

        return cedearInformation is not null
            ? cedearInformation
            : await FetchCedearInformationAndSaveInCache(financialAssetId);
    }

    private async Task<CedearInformationDto> FetchCedearInformationAndSaveInCache(Guid financialAssetId)
    {
        Cedear? cedear = await context.Cedears.FirstOrDefaultAsync(c => c.FinancialAssetId == financialAssetId);

        CedearInformationDto cedearInformation = CreateCedearInformationDto(cedear);

        await cacheService.UpdateCedearInformation(financialAssetId, cedearInformation);

        return cedearInformation;
    }

    private CedearInformationDto CreateCedearInformationDto(Cedear? cedear)
    {
        return cedear is null
            ? CreateNotExistentCedearInformationDto()
            : new CedearInformationDto { HasCedear = true, Ratio = cedear.Ratio };
    }

    private CedearInformationDto CreateNotExistentCedearInformationDto() 
        => new CedearInformationDto { HasCedear = false, Ratio = "Unkonwn Ratio" };
}
