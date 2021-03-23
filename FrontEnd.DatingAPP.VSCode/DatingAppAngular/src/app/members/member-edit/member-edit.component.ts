import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Users } from 'src/app/_models/users';
import { AuthService } from 'src/app/_services/auth.service';
import { UsersService } from 'src/app/_services/users.service';
declare let alertify:any;
@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm') editForm:NgForm;
  user:Users;
  constructor(private route:ActivatedRoute,private userservice:UsersService,private authservice:AuthService) { }

  ngOnInit() {
    console.log('here');
    this.route.data.subscribe(data=>{
      this.user=data['user'];
    })
  }
  updateUser(){
    this.userservice.updateUserInformation(this.authservice.decodedToken.UserID,this.user)
    .subscribe(next=>{
      alertify.success("Profile Updated Successfully");
      this.editForm.reset(this.user);
    },error=>{
      alertify.error(error);
    })
  }
  updateMainPhoto(photourl){
    this.user.photoUrl = photourl;
  }
}
