import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import {  FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { BsDropdownConfig } from 'ngx-bootstrap/dropdown';
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
  registerForm:FormGroup;
  bsConfig:Partial<BsDatepickerConfig>;
  constructor(private autservice:AuthService,private fb:FormBuilder,
    private authService:AuthService,private router:Router) { }

  ngOnInit() { 
    this.bsConfig={
      containerClass:'theme-red'
    }
    this.createRegisterForm();
  }
  createRegisterForm() {
    this.registerForm = this.fb.group({
      gender: ['male'],
      username: ['', Validators.required],
      knownAs: ['', Validators.required],
      dateOfBirth: [null, Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ['', Validators.required]
    }, {validator: this.passwordMatchValidator});
  }
  passwordMatchValidator(g: FormGroup) {
    return g.get('password').value === g.get('confirmPassword').value ? null : {'mismatch': true};
  }

  register(){
    if (this.registerForm.valid){
      this.autservice.register(this.registerForm.value).subscribe(response=>{
        alertify.success('Register Sucuess');
        this.authService.login(this.registerForm.value).subscribe(next => {
          alertify.success('Logged in successfully');
        }, error => {
          alertify.error(error);
        }, () => {
          this.router.navigate(['/members']);
        });

      },error=>{
        alertify.error(error);
      });
      console.log(this.registerForm.value);
    }
  }
  cancel(){
    this.cancelClicked.emit(false);
    console.log('cancelled');
  }
}
