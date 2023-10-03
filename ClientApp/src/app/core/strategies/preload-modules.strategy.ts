// Preloading example from https://angular.io/docs/ts/latest/guide/router.html#!#custom-preloading

import { Injectable } from '@angular/core';
import { PreloadingStrategy, Route } from '@angular/router';
import { Observable, of } from 'rxjs';
import { environment } from '@env/environment';

@Injectable()
export class PreloadModulesStrategy implements PreloadingStrategy {

  constructor() {}

  preload(route: Route, load: () => Observable<any>): Observable<any> {
    return (environment.production || (route.data && route.data.preload)) ? load() : of(null);
  }
}
