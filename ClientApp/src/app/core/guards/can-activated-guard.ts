import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';

import { AuthService } from '@app/core/services/auth.service';

@Injectable()
export class CanActivateGuard implements CanActivate {

  constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    return new Promise(response => {
      this.authService.getLoggedUserInfo().subscribe(
        (data) => {
          if (data.content) {
            response(true);
          } else {
            this.authService.redirectUrl = state.url;
            this.router.navigate(['/login']);
            response(false);
          }
        },
        (error) => {
          this.authService.redirectUrl = state.url;
          this.router.navigate(['/login']);
          response(false);
        }
      );
    });
  }
}
