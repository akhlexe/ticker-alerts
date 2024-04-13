using System.Collections;
using TickerAlert.Application.Interfaces.FinancialAssets;
using TickerAlert.Application.Services.FinancialAssets.Dtos;
using TickerAlert.Domain.Entities;

namespace TickerAlert.Application.Services.FinancialAssets;

public class FinancialAssetReader : IFinancialAssetReader
{
    private readonly IFinancialAssetRepository _repository;

    public FinancialAssetReader(IFinancialAssetRepository repository) => _repository = repository;

    public async Task<IEnumerable<FinancialAssetDto>> GetAllBySearchCriteria(string criteria)
    {
        var assets = await _repository.GetAllBySearchCriteria(criteria);

        return assets.Select(CreateFinancialAssetDto);
    }

    private static FinancialAssetDto CreateFinancialAssetDto(FinancialAsset financialAsset)
    {
        return new FinancialAssetDto()
        {
            Id = financialAsset.Id,
            Ticker = financialAsset.Ticker,
            Name = financialAsset.Name,
        };
    }
}