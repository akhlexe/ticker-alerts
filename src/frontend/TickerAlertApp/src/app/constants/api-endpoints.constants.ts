import { environment } from '../../environments/environment';

export const Endpoints = {
  Alerts: `${environment.apiBaseUrl}/Alerts`,
  CreateAlert: `${environment.apiBaseUrl}/Alerts/CreateAlert`,
  FinancialAssets: `${environment.apiBaseUrl}/FinancialAssets`,
};

export const AuthEndpoints = {
  Register: `${environment.apiBaseUrl}/Auth/Register`,
  Login: `${environment.apiBaseUrl}/Auth/Login`,
}
