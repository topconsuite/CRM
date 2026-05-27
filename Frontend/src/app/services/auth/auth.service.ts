import { Injectable } from '@angular/core';

import { Observable } from 'rxjs';

const lsKeyToken: string = 't.tcw.token';

@Injectable()
export class AuthService {

    constructor() { }

    getToken(): string {
        return localStorage.getItem(lsKeyToken);
    }

    setToken(token: string): void {
        localStorage.setItem(lsKeyToken, token);
    }

    deleteToken(): void {
        localStorage.removeItem(lsKeyToken);
    }

}