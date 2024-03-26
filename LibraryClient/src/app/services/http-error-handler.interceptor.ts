import { HttpEvent, HttpInterceptorFn, HttpResponse, HttpStatusCode } from '@angular/common/http';
import { inject } from '@angular/core';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { SpinnerService } from './spinner.service';
import { AlertifyService, MessageType, PositionType } from './alertify.service';

export const httpErrorHandlerInterceptor: HttpInterceptorFn = (req, next) => {
  const alertify = inject(AlertifyService);
  return next(req).pipe(map((event: HttpEvent<any>) => {
    if (event instanceof HttpResponse) {
      if(event.body.succeeded){
        alertify.message(event.body.message,MessageType.success,PositionType.bottomRight);
      }else if(event.body.succeeded == false){
        alertify.message(event.body.message,MessageType.warning,PositionType.bottomRight);
      }
    }
    return event;
  }));
};


