import { Injectable, Output, EventEmitter } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { BaseHttpService } from './base-http.service';
import { catchError, map, Observable } from 'rxjs';
import { IUserLogin } from 'src/app/shared/models/login.model';

@Injectable({ providedIn: 'root' })
export class AuthService extends BaseHttpService {
  redirectUrl: string;
  edlHeader: any;
  authData: any;

  @Output() authChanged: EventEmitter<any> = new EventEmitter<any>();

  constructor(
    private http: HttpClient
  ) {
    super();
  }

  login(userLogin: IUserLogin): Observable<any> {
    return this.http
      .post<any>(this.baseUrl + '/api/auth/login', userLogin)
      .pipe(
        map(authData => {
          return authData;
        }),
        catchError(this.handleError)
      );
  }

  logout(): Observable<any> {
    const model = {};
    return this.http.post<any>(`${this.baseUrl}/api/auth/logout`, model).pipe(
      map(response => {
        return response || null;
      }),
      catchError(this.handleError)
    );
  }

  getLoggedUserInfo() {
    return this.http
      .get<any>(this.baseUrl + '/api/user/getloggedinuserinfo')
      .pipe(
        map(response => {
          this.authData = response.content;
          return response;
        }),
        catchError(this.handleError)
      );
  }
}
