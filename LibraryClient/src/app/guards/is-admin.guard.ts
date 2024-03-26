import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Router } from 'express';

export const isAdminGuard: CanActivateFn = (route, state) => {
  var token = localStorage.getItem("AccessToken");
  if(token){
    const helper = new JwtHelperService();
    const decoded = helper.decodeToken(token);
    if(decoded.role=="admin"){
      return true;
    }
  }
  return false;
};
