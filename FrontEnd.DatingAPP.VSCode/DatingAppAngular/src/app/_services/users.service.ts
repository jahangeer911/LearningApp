import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { PaginatedResult } from '../_models/pagination';
import { Users } from '../_models/users';

 
@Injectable({
  providedIn: 'root'
})
export class UsersService {
  baseURL=environment.apiURL;
  constructor(private http:HttpClient) { }

  getAllUser(pageNumber?,itemsPerPage?,userParams?,likeParam?):Observable<PaginatedResult<Users[]>>{
    const paginatedResult:PaginatedResult<Users[]>=new PaginatedResult<Users[]>();
    let params=new HttpParams();
    if(pageNumber!=null&&itemsPerPage!=null){
      params = params.append('pageNumber',pageNumber);
      params = params.append('pageSize',itemsPerPage);
    }
    if(userParams!=null){
      params = params.append('minAge',userParams.minAge);
      params = params.append('maxAge',userParams.maxAge);
      params = params.append('gender',userParams.gender);
      params = params.append('orderBy',userParams.orderBy);
    }
    if(likeParam==='Likees'){
      params = params.append('likees','true');
    }
    if(likeParam==='Likers'){
      params = params.append('likers','true');
    }
    return this.http.get<Users[]>(this.baseURL+'users',{observe:'response',params}).pipe(
      map(response=>{
        paginatedResult.result=response.body;
        if(response.headers.get('Pagination')!=null){
          paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }
        return paginatedResult;  
      })
    );
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
  sendLike(userID:number,recepientid:number){
    return this.http.post(this.baseURL+'users/'+userID+'/like/'+recepientid,{});
  }
}
