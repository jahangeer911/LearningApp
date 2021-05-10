import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PaginatedResult, Pagination } from '../_models/pagination';
import { Users } from '../_models/users';
import { AuthService } from '../_services/auth.service';
import { UsersService } from '../_services/users.service';
declare let alertify:any;
@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {
  users:Users[];
  pagination:Pagination;
  likeParams:any={};
  constructor(private authService:AuthService,private userservice:UsersService,
    private route:ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data=>{
      this.users=data['users'].result;
      this.pagination = data['users'].pagination;
    });
    this.likeParams = 'Likers';
  }
  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadUsers();
  }
  loadUsers(){
    this.userservice.getAllUser(this.pagination.currentPage,this.pagination.itemsPerPage,null,this.likeParams).
      subscribe((response:PaginatedResult<Users[]>)=>{ 
          this.users = response.result;
          this.pagination = response.pagination;
        },error=>{
          alertify.error(error);
        });

  }

}
