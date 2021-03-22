import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

export interface User {
  username: string;
  authProps?: AuthProps;
}

interface AuthProps {
  accessToken: string;
  expiresAt: number;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUserSubject: BehaviorSubject<User>;

  constructor(private http: HttpClient) {
    const accessToken: string | null = localStorage.getItem('accessToken');
    const expiresAt: string | null = localStorage.getItem('expiresAt');
    const username: string | null = localStorage.getItem('username');
    if(accessToken && expiresAt && username) {
      const user = {
        username: username,
        authProps: {
          accessToken: accessToken,
          expiresAt: Number(expiresAt)
        }
      } as User
      this.currentUserSubject = new BehaviorSubject<User>(user);
    }
    else {
      this.currentUserSubject = new BehaviorSubject<User>({} as User)
    }
  }

  public get currentUserValue(): User {
    return this.currentUserSubject.value;
  }

  login(username: string, password: string): Observable<AuthProps> {
    let params = new HttpParams();
    params = params.append('login', username);
    params = params.append('password', password);

    return this.http.post<AuthProps>(`${environment.apiUrl}/api/identity/login?login=${username}&password=${password}`, params)
        .pipe(map(authProps => {
            localStorage.setItem('accessToken', JSON.stringify(authProps.accessToken));
            localStorage.setItem('expiresAt', JSON.stringify(authProps.expiresAt));
            localStorage.setItem('username', JSON.stringify(username));
            const user = {
              username: username,
              authProps
            } as User
            this.currentUserSubject.next(user);
            return authProps;
        }));
  }

  logout(): void {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('expiresAt');
    localStorage.removeItem('username');
    this.currentUserSubject.next({} as User);
  }
}
