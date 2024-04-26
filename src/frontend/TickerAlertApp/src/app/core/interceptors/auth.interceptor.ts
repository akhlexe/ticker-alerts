import { HttpInterceptorFn } from '@angular/common/http';

export const authInterceptor: HttpInterceptorFn = (req, next) => {

  const token: string | null = localStorage.getItem('authToken');

  let request = req;

  if (token) {
    request = req.clone({
      setHeaders: {
        authorization: `Bearer ${token}`
      }
    });
  }

  return next(request);
};
