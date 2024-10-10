import {catchError, Observable, throwError} from "rxjs";
import {HttpEvent, HttpHandlerFn, HttpInterceptorFn, HttpRequest, HttpStatusCode} from "@angular/common/http";
import {inject, Injector} from "@angular/core";
import {GlobalMessageService} from "../services/global-message.service";
import {Router} from "@angular/router";
import { errorKeys } from "../utils/error-keys";

export const errorInterceptor: HttpInterceptorFn = (req: HttpRequest<any>, next: HttpHandlerFn): Observable<HttpEvent<any>> => {
  const globalMessageService: GlobalMessageService = inject(GlobalMessageService);
  const injector: Injector = inject(Injector);

  const handleError = (errors: any, statusCode: number): void => {
    switch (statusCode) {
      case HttpStatusCode.BadRequest:
        for (let errorKey of errorKeys(errors))
          globalMessageService.showInformationMessage({title: '', content: `${errorKey}`})
        break;
      case HttpStatusCode.InternalServerError:
        break;
      case HttpStatusCode.NotFound:
        injector.get(Router).navigate(['/not-found']);
        break;
      default:
        break;
    }
  }

  return next(req).pipe(catchError(err => {
    handleError(err.error.errors, err.status);
    return throwError(() => err)
  }));
};
