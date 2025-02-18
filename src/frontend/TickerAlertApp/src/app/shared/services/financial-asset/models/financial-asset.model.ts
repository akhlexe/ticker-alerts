export interface FinancialAssetDto {
  id: string;
  ticker: string;
  name: string;
}

export interface FinancialAssetProfileDto {
  profile: CompanyProfileDto,
  cedearCotizacion: CedearCotizacion
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

export interface CedearCotizacion {
  ratio: string,
  hasCedear: boolean,
  cedearCompra: number,
  cedearVenta: number
}