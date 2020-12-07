import { Component, OnInit } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { AuthService } from '../_services/auth.service';

declare let alertify:any;
@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model:any={};
  jwtHelper =new JwtHelperService();
  constructor(private autservice:AuthService) { }

  ngOnInit() {
    this.autservice.decodedToken = this.jwtHelper.decodeToken(localStorage.getItem('token'));
  }

  login(){
    this.autservice.login(this.model).subscribe(next=>{
       alertify.success('success');
    },error=>{
      alertify.error(error);
    }); 
    console.log(this.model);
  }
  loggedid(){
    return this.autservice.loggedin();
  }
  logout(){
    localStorage.removeItem('token');
    alertify.message('logout success');
  }
}
