import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';

import { UserService } from './../user.service';

@Injectable()
export class AuthGuardService implements CanActivate {

  constructor(private router: Router, private userService: UserService) { }

  canActivate() {
        if ( this.userService.isLogged() ) {
            return true;
        }

        this.router.navigate(['pages/auth/login']);
        return false;
    }

}
