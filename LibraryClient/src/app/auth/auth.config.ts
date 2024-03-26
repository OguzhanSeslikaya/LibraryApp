import { PassedInitialConfig } from 'angular-auth-oidc-client';

export const authConfig: PassedInitialConfig = {
  config: {
    authority: "https://localhost:7045",
    clientId: "AngularClient",
    redirectUrl: "http://localhost:4200/callback",
    postLogoutRedirectUri: "http://localhost:4200",
    responseType: "code",
    scope: "LoanAPI.read LoanAPI.write StockAPI.read StockAPI.write profile openid role",
    
          }
}
