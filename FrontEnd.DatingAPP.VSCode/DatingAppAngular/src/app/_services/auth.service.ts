import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseURL = 'http://localhost:5000/api/Auth/';

  constructor(private http:HttpClient) { }

  login(model:any){
    return this.http.post(this.baseURL+'login',model)
      .pipe(
          map((response:any)=>{
            const login =response;
            if(login){
              localStorage.setItem('token',login.token);
            }
          })
      )
  }
  register(model:any){
    return this.http.post(this.baseURL+'register',model)
  }
}
