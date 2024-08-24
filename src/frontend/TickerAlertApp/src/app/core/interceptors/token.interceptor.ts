import { HttpInterceptorFn } from '@angular/common/http';
import { AuthService } from '../services/auth.service';
import { inject } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

export const tokenInterceptor: HttpInterceptorFn = (req, next) => {

  const authService = inject(AuthService);

  if (authService.isAuth() && authService.isTokenExpired()) {

    const toastrService = inject(ToastrService);

    authService.logout();
    toastrService.error(
      "Session Expired, please, log again...",
      "Session",
      {
        closeButton: true,
        positionClass: "toast-bottom-left",
        progressBar: true,
      })
  }

  return next(req);
};
