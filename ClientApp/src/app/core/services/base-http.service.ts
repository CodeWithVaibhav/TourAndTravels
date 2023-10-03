import { Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';

import { throwError } from 'rxjs';
import { environment } from '@env/environment';

@Injectable()
export class BaseHttpService {
  protected baseUrl = environment.baseURL; // 'http://localhost:9011';

  constructor() {
    this.setBaseUrl();
  }

  protected handleError(error: HttpErrorResponse) {
    console.error('server error:', error);
    if (error.error instanceof Error) {
      const errMessage = error.error.message;
      return throwError(() => new Error(errMessage));
      // Use the following instead if using lite-server
      // return Observable.throw(err.text() || 'backend server error');
    }
    return throwError(() => new Error(error.error.message || 'Node.js server error'));
  }

  private setBaseUrl() {
    if (environment.production) {
      const baseUrl = document.getElementsByTagName('base')[0].href;
      this.baseUrl = baseUrl.charAt(baseUrl.length - 1) === '/' ?
        baseUrl.slice(0, -1) :
        baseUrl;
    } else {
      this.baseUrl = environment.baseURL.charAt(environment.baseURL.length - 1) === '/' ?
        environment.baseURL.slice(0, -1) :
        environment.baseURL;
    }
  }
}
