export interface Alert {
  tickerName: string;
  targetPrice: number;
  actualPrice: number;
  difference: number;
  state: string;
}
