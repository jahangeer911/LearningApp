import { Component, Input, OnInit } from '@angular/core';
import { Users } from 'src/app/_models/users';
import { AuthService } from 'src/app/_services/auth.service';
import { UsersService } from 'src/app/_services/users.service';
declare let alertify:any;
@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {
  @Input() user:Users;
  constructor(private userservice:UsersService,private authservice:AuthService) { }

  ngOnInit() {
    //  
  }

  sendLike(id:number){
    this.userservice.sendLike(this.authservice.decodedToken.UserID,id).subscribe(data=>{
      alertify.success('Liked:' + this.user.knownAs);
    },error=>{
      alertify.error(error);
    });
  }

}
