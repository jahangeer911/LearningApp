import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, Router, RouterStateSnapshot } from "@angular/router";
import { Observable, of } from "rxjs";
import { catchError } from "rxjs/operators";
import { Users } from "../_models/users";
import { AuthService } from "../_services/auth.service";
import { UsersService } from "../_services/users.service";
declare let alertify:any;
@Injectable()
export class MemberEditResolver implements Resolve<Users>{
    constructor(private userservice:UsersService,private router:Router,private authservice:AuthService){}

    resolve(route: ActivatedRouteSnapshot): Observable<Users>  { 
        return this.userservice.getUserbyID(this.authservice.decodedToken.UserID).pipe(
            catchError(error=>{
                alertify.error(error);
                console.log(error);
                this.router.navigate(['/members']);
                return of(null);
            })
        )
    }

}