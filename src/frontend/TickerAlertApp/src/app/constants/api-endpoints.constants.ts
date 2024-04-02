import { environment } from '../../environments/environment';

export const Endpoints = {
  Alerts: `${environment.apiBaseUrl}/Alerts`,
  CreateAlert: `${environment.apiBaseUrl}/Alerts/CreateAlert`,
};
