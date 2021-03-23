import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { JwtHelperService } from "@auth0/angular-jwt";
import { environment } from 'src/environments/environment';
import { Users } from '../_models/users';
import {BehaviorSubject} from  'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseURL = environment.apiURL+'Auth/';
  jwtHelper = new JwtHelperService();
  decodedToken :any;
  currentUser:Users;
  photoURLfromAuthService =new BehaviorSubject<string>('../../assets/user.png');
  currentPhotoUrl = this.photoURLfromAuthService.asObservable(); 

  constructor(private http:HttpClient) { }

  changeMemberPhoto(photoURLpassed:string){
    this.photoURLfromAuthService.next(photoURLpassed);
  }
  login(model:any){
    return this.http.post(this.baseURL+'login',model)
      .pipe(
          map((response:any)=>{
            const login =response;
            if(login){
              localStorage.setItem('token',login.token);
              localStorage.setItem('user',JSON.stringify(login.user));
              this.decodedToken = this.jwtHelper.decodeToken(login.token);
              this.currentUser = login.user;
              this.changeMemberPhoto(this.currentUser.photoUrl);
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
