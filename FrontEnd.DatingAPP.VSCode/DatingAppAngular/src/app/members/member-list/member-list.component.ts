import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
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
  constructor(private userservice:UsersService,private route :ActivatedRoute) { }
  
  ngOnInit() {
    this.route.data.subscribe(data=>{
      this.users=data['users'];
    })
    //this.loadUsers();
  }
  loadUsers(){
    this.userservice.getAllUser().subscribe((returnedusersfromapi:Users[])=>{
      this.users=returnedusersfromapi;
      
    },error=>{
      alertify.error(error);
    })

  }

}
