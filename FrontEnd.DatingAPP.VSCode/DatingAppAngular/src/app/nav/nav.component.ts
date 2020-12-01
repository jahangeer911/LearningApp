import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
declare let alertify:any;
@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model:any={};
  constructor(private autservice:AuthService) { }

  ngOnInit() {
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
    const token =localStorage.getItem('token');
    return !!token;
  }
  logout(){
    localStorage.removeItem('token');
  }
}
