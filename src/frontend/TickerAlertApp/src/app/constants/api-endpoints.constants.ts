import { environment } from '../../environments/environment';

export const Endpoints = {
  Alerts: `${environment.apiProxy}/Alerts`,
  CreateAlert: `${environment.apiProxy}/Alerts/CreateAlert`,
  FinancialAssets: `${environment.apiProxy}/FinancialAssets`,
};

export const AuthEndpoints = {
  Register: `${environment.apiProxy}/Auth/Register`,
  Login: `${environment.apiProxy}/Auth/Login`,
}
