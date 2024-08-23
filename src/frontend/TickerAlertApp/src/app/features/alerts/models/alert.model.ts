import { AlertState } from "./alert-state.enum";

export interface Alert {
  id: number;
  tickerName: string;
  targetPrice: number;
  actualPrice: number;
  difference: number;
  state: AlertState;
}
