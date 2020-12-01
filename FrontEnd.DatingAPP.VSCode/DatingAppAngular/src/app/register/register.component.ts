import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AuthService } from '../_services/auth.service';
declare let alertify:any;
@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelClicked =new EventEmitter();
  model:any={} ;
  
  constructor(private autservice:AuthService) { }

  ngOnInit() { 
  }
  register(){
    this.autservice.register(this.model).subscribe(response=>{
      alertify.success("Success in Register");
    },error=>{
      alertify.error(error);
    })
    console.log(this.model);
  }
  cancel(){
    this.cancelClicked.emit(false);
    console.log('cancelled');
  }
}
