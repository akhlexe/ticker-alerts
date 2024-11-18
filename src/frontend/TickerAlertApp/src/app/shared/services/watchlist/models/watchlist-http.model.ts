export interface AddItemRequest {
    watchlistId: string;
    financialAssetId: string;
}

export interface RemoveItemRequest {
    watchlistId: string;
    itemId: string;
}