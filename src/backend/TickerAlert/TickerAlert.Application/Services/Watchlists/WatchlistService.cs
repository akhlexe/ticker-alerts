using Microsoft.EntityFrameworkCore;
using TickerAlert.Application.Common.Persistence;
using TickerAlert.Application.Interfaces.Authentication;
using TickerAlert.Application.Interfaces.FinancialAssets;
using TickerAlert.Application.Interfaces.Watchlists;
using TickerAlert.Application.Interfaces.Watchlists.Dtos;
using TickerAlert.Application.Services.Watchlists.Mappings;
using TickerAlert.Domain.Entities;


namespace TickerAlert.Application.Services.Watchlists;
internal sealed class WatchlistService(
    IApplicationDbContext context,
    ICurrentUserService currentUserService,
    IFinancialAssetReader assetsReader) : IWatchlistService
{
    public async Task<WatchlistDto> GetWatchlist()
    {
        Watchlist? watchlist = await context
            .Watchlists
            .Include(w => w.WatchlistItems)
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.UserId == currentUserService.UserId);

        if (watchlist == null)
        {
            Watchlist watchlistCreated = await CreateWatchlist();
            return WatchlistMappings.MapToDto(watchlistCreated, []);
        }

        return await ReturnWatchlistDto(watchlist);
    }

    public async Task<WatchlistDto> AddItem(Guid watchlistId, Guid financialAssetId)
    {
        Watchlist watchlist = await GetWatchlistById(watchlistId);

        if (watchlist.WatchlistItems.All(x => x.FinancialAssetId != financialAssetId))
        {
            var item = WatchlistItem.Create(Guid.NewGuid(), watchlistId, financialAssetId);
            context.WatchlistItems.Add(item);

            await context.SaveChangesAsync();
        }

        return await ReturnWatchlistDto(watchlist);
    }

    public async Task<WatchlistDto> RemoveItem(Guid watchlistId, Guid itemId)
    {
        Watchlist watchlist = await GetWatchlistById(watchlistId);

        if (watchlist.WatchlistItems.Exists(x => x.Id == itemId))
        {
            WatchlistItem item = watchlist.WatchlistItems.Find(x => x.Id == itemId)!;
            watchlist.WatchlistItems.Remove(item);

            context.Watchlists.Update(watchlist);
            await context.SaveChangesAsync();
        }

        return await ReturnWatchlistDto(watchlist);
    }

    private async Task<Watchlist> GetWatchlistById(Guid watchlistId)
    {
        var watchlist = await context
            .Watchlists
            .Include(w => w.WatchlistItems)
            .FirstOrDefaultAsync(w => w.Id == watchlistId);

        return watchlist ?? throw new InvalidOperationException($"Watchlist with ID {watchlistId} does not exist.");
    }

    private async Task<Watchlist> CreateWatchlist()
    {
        Watchlist watchlist = Watchlist.Create(Guid.NewGuid(), currentUserService.UserId);

        context.Watchlists.Add(watchlist);
        await context.SaveChangesAsync();

        return watchlist;
    }

    private async Task<WatchlistDto> ReturnWatchlistDto(Watchlist watchlist)
    {
        List<Guid> financialAssetIds = watchlist.WatchlistItems.Select(w => w.FinancialAssetId).ToList();
        var assets = await assetsReader.GetAllByIds(financialAssetIds);

        return WatchlistMappings.MapToDto(watchlist, assets.ToDictionary(a => a.Id, a => a));
    }
}
