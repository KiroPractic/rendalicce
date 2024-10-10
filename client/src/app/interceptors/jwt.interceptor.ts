import {HttpEvent, HttpHandlerFn, HttpInterceptorFn, HttpRequest} from "@angular/common/http";
import {Observable} from "rxjs";
import {inject} from "@angular/core";
import {JwtService} from "../services/jwt.service";

export const jwtInterceptor: HttpInterceptorFn = (req: HttpRequest<any>, next: HttpHandlerFn): Observable<HttpEvent<any>> => {
  const jwtService: JwtService = inject(JwtService);

  if (jwtService.isTokenExpired()) {
    jwtService.remove()
    return next(req);
  }

  const cloned: HttpRequest<any> = req.clone({
    setHeaders: {
      "Authorization": `Bearer ${jwtService.rawToken()}`,
    },
  });

  return next(cloned);
};
