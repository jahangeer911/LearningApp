import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';

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
        console.log('success');
    },error=>{
      console.log('fail');
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
