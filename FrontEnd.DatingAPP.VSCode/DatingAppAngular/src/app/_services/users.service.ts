import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Users } from '../_models/users';

 
@Injectable({
  providedIn: 'root'
})
export class UsersService {
  baseURL=environment.apiURL;
  constructor(private http:HttpClient) { }

  getAllUser():Observable<Users[]>{
    return this.http.get<Users[]>(this.baseURL+'users');
  }
  getUserbyID(id):Observable<Users>{
    return this.http.get<Users>(this.baseURL+'users/'+id)
  }
  updateUserInformation(id:number,user:Users){
    return this.http.put(this.baseURL+'users/'+id,user);
  }
  setMainPhoto(userid:number,photoid:number){
    return this.http.post(this.baseURL+'users/'+userid+'/photos/'+photoid+'/setMain',{});
  }
  deletePhoto(userId: number, id: number) {
    return this.http.delete(this.baseURL + 'users/' + userId + '/photos/' + id);
  }
}
