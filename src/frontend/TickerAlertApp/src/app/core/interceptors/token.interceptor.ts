import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { NotificationService } from '../services/notification/notification.service';

export const tokenInterceptor: HttpInterceptorFn = (req, next) => {

  const authService = inject(AuthService);

  if (authService.isAuth() && authService.isTokenExpired()) {
    const notificationService = inject(NotificationService);
    authService.logout();
    notificationService.showError('Session Expired', 'Session');
  }

  return next(req);
};
