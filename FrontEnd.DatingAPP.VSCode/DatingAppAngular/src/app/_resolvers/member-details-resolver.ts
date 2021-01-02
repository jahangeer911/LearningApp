import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, Router, RouterStateSnapshot } from "@angular/router";
import { Observable, of } from "rxjs";
import { catchError } from "rxjs/operators";
import { Users } from "../_models/users";
import { UsersService } from "../_services/users.service";
declare let alertify:any;
@Injectable()
export class MemberDetailResolver implements Resolve<Users>{
    constructor(private userservice:UsersService,private router:Router){}

    resolve(route: ActivatedRouteSnapshot): Observable<Users>  {
        return this.userservice.getUserbyID(route.params['id']).pipe(
            catchError(error=>{
                alertify.error(error);
                this.router.navigate(['/members']);
                return of(null);
            })
        )
    }

}