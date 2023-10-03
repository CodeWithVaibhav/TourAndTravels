import { Injectable, Output, EventEmitter } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseHttpService } from './base-http.service';
import { catchError, map, Observable } from 'rxjs';
import { IUserLogin } from 'src/app/shared/models/login.model';
import { StorageService } from './storage.service';
import { ICommonInput } from '@app/shared/models/commoninput.model';
import { IDeleteUser } from '@app/shared/models/deleteusermodel';

@Injectable({ providedIn: "root" })
export class ADService extends BaseHttpService {
  redirectUrl: string;
  edlHeader: any;

  constructor(
    private http: HttpClient,
    private storageService: StorageService
  ) {
    super();
  }

  getAllGroupsCount(): Observable<any> {
    return this.http
      .get<any>(this.baseUrl + "/api/ad/getallgroupscount", {})
      .pipe(
        map((response) => {
          if (!response.isError) {
          }
          return response;
        }),
        catchError(this.handleError)
      );
  }

  getAllGroups(model: ICommonInput): Observable<any> {
    return this.http
      .post<any>(this.baseUrl + "/api/ad/getallgroups", model)
      .pipe(
        map((response) => {
          if (!response.isError) {
          }
          return response;
        }),
        catchError(this.handleError)
      );
  }

  getAllUsers(model: ICommonInput): Observable<any> {
    return this.http
      .post<any>(this.baseUrl + "/api/ad/getallusers", model)
      .pipe(
        map((response) => {
          if (!response.isError) {
          }
          return response;
        }),
        catchError(this.handleError)
      );
  }

  createUser(model: IDeleteUser): Observable<any> {
    return this.http.post<any>(this.baseUrl + "/api/ad/createuser", model).pipe(
      map((response) => {
        if (!response.isError) {
        }
        return response;
      }),
      catchError(this.handleError)
    );
  }

  deleteUser(model: IDeleteUser): Observable<any> {
    return this.http.post<any>(this.baseUrl + "/api/ad/deleteuser", model).pipe(
      map((response) => {
        if (!response.isError) {
        }
        return response;
      }),
      catchError(this.handleError)
    );
  }
}
