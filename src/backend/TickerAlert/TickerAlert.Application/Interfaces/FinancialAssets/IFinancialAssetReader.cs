using TickerAlert.Application.Services.FinancialAssets.Dtos;

namespace TickerAlert.Application.Interfaces.FinancialAssets;

public interface IFinancialAssetReader
{
    Task<IEnumerable<FinancialAssetDto>> GetAllBySearchCriteria(string criteria);
}