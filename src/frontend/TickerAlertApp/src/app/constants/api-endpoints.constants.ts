import { environment } from '../../environments/environment';

export const Endpoints = {
  Alerts: `${environment.apiProxy}/Alerts`,
  CreateAlert: `${environment.apiProxy}/Alerts/CreateAlert`,
  CancelAlert: `${environment.apiProxy}/Alerts/CancelAlert`,
  ConfirmReception: `${environment.apiProxy}/Alerts/ConfirmReception`,
  FinancialAssets: `${environment.apiProxy}/FinancialAssets`,
  FinancialAssetProfile: `${environment.apiProxy}/FinancialAssets/Profile`,
};

export const AuthEndpoints = {
  Register: `${environment.apiProxy}/Auth/Register`,
  Login: `${environment.apiProxy}/Auth/Login`,
}
