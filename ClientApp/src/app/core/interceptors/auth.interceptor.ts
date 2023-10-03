import { Injectable } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  anynomouseRequests = [];

  constructor(
    private authService: AuthService,
    private router: Router,
    private toastService: ToastrService
  ) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    req = req.clone({
      setHeaders: {
        'Content-Type': 'application/json',
        'Encoding-Content': `gzip`
      }
    });

    return next.handle(req).pipe(tap((ev: HttpEvent<any>) => {
      if (ev instanceof HttpResponse) {
        const token = ev.headers.get('X-Token') || '';
        if (token) {
          // this.authService.updateToken(token);
        }
      }
    }),
      catchError((response, caught) => {
        if (response.status === 401) {
          this.router.navigate(['/login']);
          return throwError(() => response)
        }

        if (response instanceof HttpErrorResponse) {
          if (response.status === 404) {
            console.log(response.statusText || 'Resource not found')
          } else if (response.status === 200) {
            if (response['error']['text'] &&
              (response['error']['text'].indexOf('SAMLRequest') > 0 || response['error']['text'].indexOf('SAMLResponse') > 0)) {
              const samlDiv = document.getElementById('samldiv') as any;
              samlDiv.innerHTML = response['error']['text'];
              const f1 = samlDiv.children[1];
              f1.submit();
            } else {
              console.log(response.error.text)
            }
          } else {
            console.log(response.error.title || response.error.error || 'Internal server error');
            this.toastService.error(response.error.title || response.error.error);
            return throwError(() => response)
          }
        }
        return throwError(() => response)
      })
    );
  }
}
