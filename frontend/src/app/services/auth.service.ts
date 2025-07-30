import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {map, Observable, tap} from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthService {
    private tokenUrl = 'http://localhost:8080/api/Auth/token';

    constructor(private http: HttpClient) {}

    login(): Observable<void> {
        const credentials = {
            clientId: 'fundo-app',
            clientSecret: 'dev-secret-123'
        };

        return this.http.post<{ accessToken: string, expiresIn: number }>(
            this.tokenUrl,
            credentials
        ).pipe(
            tap((res) => {
                console.log('📦 Token recebido da API:', res.accessToken);
                localStorage.setItem('access_token', res.accessToken);
            }),
            map(() => {}) // retorno void
        );
    }

    getToken(): string | null {
        const token = localStorage.getItem('access_token');
        console.log('🔑 getToken():', token);
        return token;
    }
}
