using Microsoft.EntityFrameworkCore;
using TickerAlert.Application.Common.Persistence;
using TickerAlert.Application.Interfaces.Cedears.Dtos;
using TickerAlert.Domain.Entities;

namespace TickerAlert.Application.Services.Cedears;

public sealed class CedearInformationService
{
    private readonly CedearInformationCacheService _cacheService;
    private readonly IApplicationDbContext _context;

    public CedearInformationService(CedearInformationCacheService cacheService, IApplicationDbContext context)
    {
        _cacheService = cacheService;
        _context = context;
    }

    public async Task<CedearInformationDto> GetCedearInformationAsync(Guid financialAssetId)
    {
        var cedearInformation = await _cacheService.GetCedearInformation(financialAssetId);

        return cedearInformation is not null
            ? cedearInformation
            : await FetchCedearInformationAndSaveInCache(financialAssetId);
    }

    private async Task<CedearInformationDto> FetchCedearInformationAndSaveInCache(Guid financialAssetId)
    {
        Cedear? cedear = await _context.Cedears.FirstOrDefaultAsync(c => c.FinancialAssetId == financialAssetId);

        CedearInformationDto cedearInformation = CreateCedearInformationDto(cedear);

        await _cacheService.UpdateCedearInformation(financialAssetId, cedearInformation);

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
