import { AlertState } from "./alert-state.enum";

export interface Alert {
  id: number;
  financialAssetId: string;
  tickerName: string;
  targetPrice: number;
  actualPrice: number;
  difference: number;
  state: AlertState;
}
