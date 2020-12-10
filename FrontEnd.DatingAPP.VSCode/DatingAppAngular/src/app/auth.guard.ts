import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { CanActivate } from '@angular/router';
import { AlertifyService } from './_services/alertify.service';
import { AuthService } from './_services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private authService:AuthService,private alerify:AlertifyService,private router:Router) {}
  
  canActivate():boolean{
    if(this.authService.loggedin()){
      return true;
    }
    this.alerify.error("Unauthorized Access");
    this.router.navigate(['/home']);
    return false;
  }
}
