import { JsonPipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PaginatedResult, Pagination } from 'src/app/_models/pagination';
import { Users } from '../../_models/users';
import { UsersService } from '../../_services/users.service';
declare let alertify:any;
@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  users:Users[];
  pagination:Pagination;
  user:Users = JSON.parse(localStorage.getItem('user'));
  genderList=[{value:'male',display:'Males'},{value:'female',display:'Females'}];
  userParams:any={};
  constructor(private userservice:UsersService,private route :ActivatedRoute) { }
  
  ngOnInit() { 
    
    this.route.data.subscribe(data=>{
      this.users=data['users'].result;
      this.pagination=data['users'].pagination;
    });
    this.userParams.gender = this.user.gender==='female'?'male':'female';
    this.userParams.minAge =18;
    this.userParams.maxAge =99;
    this.userParams.orderBy ='lastActive';
    //this.loadUsers();
  }
  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadUsers();
  }
  resetFilters(){
    this.userParams.gender = this.user.gender==='female'?'male':'female';
    this.userParams.minAge =18;
    this.userParams.maxAge =99;
    this.userParams.orderBy ='lastActive';
    this.loadUsers();
  }
  loadUsers(){
    this.userservice.getAllUser(this.pagination.currentPage,this.pagination.itemsPerPage,this.userParams).
      subscribe((response:PaginatedResult<Users[]>)=>{ 
          this.users = response.result;
          this.pagination = response.pagination;
        },error=>{
          alertify.error(error);
        });

  }

}
