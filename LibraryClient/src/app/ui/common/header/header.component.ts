import { Component } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss',
  host: {ngSkipHydration: 'true'}
})
export class HeaderComponent {
  public _isAuthenticated:boolean=false;
  public _isAdmin:boolean=false;

  constructor(public oidcSecurityService: OidcSecurityService,private router:Router) {}

  async ngOnInit() {
    this.oidcSecurityService.checkAuth().subscribe(({ isAuthenticated, userData}) => {
      this._isAuthenticated = isAuthenticated;
      if(!isAuthenticated){
        this.oidcSecurityService.authorize();
      }
      this.oidcSecurityService.getAccessToken().subscribe(a => {
        if(a){
          if(typeof localStorage !== 'undefined'){
            localStorage.setItem("AccessToken", a);
            const helper = new JwtHelperService();
            const decoded = helper.decodeToken(a);
            this._isAdmin = (decoded.role=="admin"?true:false);
          }
        }
      });
    });
    if(!this._isAuthenticated){
      this._isAdmin = this._isAuthenticated;
    }
  }

  login() {
    this.oidcSecurityService.authorize();
  }

  logout() {
    this.oidcSecurityService.logoff().subscribe();
    localStorage.removeItem("AccessToken");
  }
 


}
