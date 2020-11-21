import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AuthService } from '../_services/auth.service';

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
    console.log(this.model);
    this.autservice.register(this.model).subscribe(response=>{
        console.log("Success in Register");
    },error=>{
      console.log(error);
    })
    console.log(this.model);
  }
  cancel(){
    this.cancelClicked.emit(false);
    console.log('cancelled');
  }
}
