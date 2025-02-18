export interface WatchlistDto {
    id: string;
    name: string;
    items: WatchlistItemDto[]
}

export interface WatchlistItemDto {
    id: string;
    watchlistId: string;
    financialAssetId: string;
    ticker: string;
    price: number;
    chartLink: string;
}