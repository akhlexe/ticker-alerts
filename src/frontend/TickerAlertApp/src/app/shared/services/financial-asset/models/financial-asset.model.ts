export interface FinancialAssetDto {
  id: string;
  ticker: string;
  name: string;
}

export interface CompanyProfileDto {
  country: string;
  currency: string;
  exchange: string;
  ipo: string;
  marketCapitalization: number;
  name: string;
  phone: string;
  shareOutstanding: number;
  ticker: string;
  weburl: string;
  logo: string;
  finnhubIndustry: string;
}