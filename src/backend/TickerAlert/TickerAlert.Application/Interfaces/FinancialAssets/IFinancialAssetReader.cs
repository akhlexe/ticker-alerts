using TickerAlert.Application.Common.Responses;
using TickerAlert.Application.Services.FinancialAssets.Dtos;
using TickerAlert.Domain.Entities;

namespace TickerAlert.Application.Interfaces.FinancialAssets;

public interface IFinancialAssetReader
{
    Task<IEnumerable<FinancialAssetDto>> GetAllBySearchCriteria(string criteria);
    Task<IEnumerable<FinancialAsset>> GetAllWithPendingAlerts();
    Task<Result<FinancialAssetDto>> GetById(Guid id);
    Task<List<FinancialAssetDto>> GetAllByIds(IEnumerable<Guid> ids);
}