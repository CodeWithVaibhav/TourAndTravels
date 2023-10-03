import { Injectable } from '@angular/core';

@Injectable()
export class StorageService {
  prefix = 'trimbec.app.';
  storage: any = {};

  constructor() { }

  getKey(key:string) {
    return `${this.prefix}${key}`;
  }
  setItem(key: string, value: string) {
    if (!key) { return; }
    sessionStorage.setItem(this.getKey(key), value);
  }

  getItem(key: string) {
    if(!this.hasItem(key)){
      return null;
    }

    return sessionStorage[this.getKey(key)];
  }

  removeAll() {
    sessionStorage.clear();
  }

  removeItem(key: string) {
    sessionStorage.removeItem(this.getKey(key));
  }

  hasItem(key: string) {
    const item = sessionStorage.getItem(this.getKey(key));
    return item !== undefined && item !== null;
  }
}
