import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideClientHydration } from '@angular/platform-browser';
import { HTTP_INTERCEPTORS, provideHttpClient, withFetch, withInterceptors } from '@angular/common/http';
import { provideAuth } from 'angular-auth-oidc-client';
import { authConfig } from './auth/auth.config';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { httpErrorHandlerInterceptor } from './services/http-error-handler.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [provideRouter(routes), provideClientHydration(), 
    provideHttpClient(withFetch(),withInterceptors([httpErrorHandlerInterceptor])), provideAuth(authConfig), 
    provideAnimationsAsync()
  ]
};
