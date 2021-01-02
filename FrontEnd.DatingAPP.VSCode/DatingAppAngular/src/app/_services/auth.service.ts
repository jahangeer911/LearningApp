import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { JwtHelperService } from "@auth0/angular-jwt";
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseURL = environment.apiURL+'Auth/';
  jwtHelper = new JwtHelperService();
  decodedToken :any;
  constructor(private http:HttpClient) { }

  login(model:any){
    return this.http.post(this.baseURL+'login',model)
      .pipe(
          map((response:any)=>{
            const login =response;
            if(login){
              localStorage.setItem('token',login.token);
              this.decodedToken = this.jwtHelper.decodeToken(login.token);
              console.log(this.decodedToken);
            }
          })
      )
  }
  register(model:any){
    console.log('called first time');
    return this.http.post(this.baseURL+'register',model)
  }
  loggedin(){
    const token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);

  }
}
